using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator
{
    class Simulation
    {
        public static readonly Time MinTimeStep = 100.Milliseconds();

        private static readonly int[] carLengthDistribution = new int[] { 2, 3, 3, 3, 3, 4, 4, 10, 10, 15 };

        private class SimulationClock : IClock
        {
            public Time Time { get; private set; }

            public void Tick(Time interval) => Time += interval;

            public void Reset() => Time = new Time();
        }

        private readonly Random random;
        private readonly SimulationClock clock;
        private Map map;
        private SimulationSettings settings;
        private CentralNavigation centralNavigation;
        private IEnumerator<Crossroad> randomCrossroads;
        private ICollection<Car> allCars;
        private HashSet<Car> stagedCars;
        private GlobalStatistics statistics;

        public bool IsRunning { get => settings != null && Clock.Time < settings.Duration; }
        public IClock Clock { get => clock; }
        public StatisticsCollector StatsCollector { get; private set; }
        public IGlobalStatistics Statistics { get => statistics; }

        public Simulation()
        {
            random = new Random();
            clock = new SimulationClock();
        }

        public enum InitialisationResult { Ok, Error_MapIsNull, Error_NoMap, Error_InvalidCrossroad }

        public InitialisationResult Initialise(Map map, out Crossroad invalidCrossroad)
        {
            clock.Reset();
            settings = null;
            invalidCrossroad = null;
            if (map == null)
                return InitialisationResult.Error_MapIsNull;
            if (map.CrossroadCount == 0 || map.RoadCount == 0)
                return InitialisationResult.Error_NoMap;
            foreach (Crossroad c in map.GetNodes())
                if (!c.Initialise())
                {
                    invalidCrossroad = c;
                    return InitialisationResult.Error_InvalidCrossroad;
                }
            this.map = map;

            StatsCollector = new StatisticsCollector();
            statistics = new GlobalStatistics(StatsCollector, Clock);
            allCars = new List<Car>();

            foreach (Road r in this.map.GetEdges())
                r.Initialise(StatsCollector, Clock);
            centralNavigation = new CentralNavigation(this.map, Clock);
            randomCrossroads = GetRandomCrossroads().GetEnumerator();
            stagedCars = new HashSet<Car>();
            return InitialisationResult.Ok;
        }

        public void StartSimulation(SimulationSettings settings, out Func<Time, bool> continueFunc)
        {
            Debug.Assert(map != null);
            this.settings = settings;
            StatisticsBase.detailSetting = settings.StatisticsDetail;
            continueFunc = ContinueSimulation;
        }

        private bool ContinueSimulation(Time duration)
        {
            Time end = Clock.Time + duration;
            if (end > settings.Duration)
                end = settings.Duration;
            while (Clock.Time < end)
            {
                Tick(settings.TimeStep);
            }
            return Clock.Time < settings.Duration;
        }

        private void Tick(Time timeStep)
        {
            // Generate new cars
            double carsPerSecond = settings.GetCarSpawnFrequency(Clock.Time) * map.CrossroadCount;
            // Not using TimeStep.ToSeconds() to achieve better precision
            double newCarProbability = carsPerSecond * timeStep / Time.precision;
            for (; newCarProbability > 0; newCarProbability--)
                GenerateCar(settings.ActiveNavigationRate, newCarProbability);
            // Release waiting cars
            HashSet<Car> releasedCars = new HashSet<Car>();
            foreach (Car c in stagedCars)
                if (c.Initialise())
                    releasedCars.Add(c);
            stagedCars.ExceptWith(releasedCars);
            // Simulation step
            clock.Tick(timeStep);
            foreach (Crossroad c in map.GetNodes())
                c.Tick(timeStep);
            foreach (Road r in map.GetEdges())
                r.Tick(timeStep);
            // Update statistics
            int finishedCars = 0;
            int carsWithZeroSpeed = 0;
            Speed speedSum = new Speed(0);
            Time delaySum = new Time(0);
            foreach (Car car in allCars)
            {
                if (car.Finished)
                {
                    finishedCars++;
                    Time delay = car.Statistics.Duration - car.Statistics.ExpectedDuration;
                    // TODO: decide what to do with negative delay (active navigation only)
                    delaySum += delay;
                }
                else
                {
                    car.AfterTick();
                    if (car.CurrentSpeed == 0)
                        carsWithZeroSpeed++;
                    speedSum += car.CurrentSpeed;
                }
            }
            int activeCars = allCars.Count - stagedCars.Count - finishedCars;
            carsWithZeroSpeed -= stagedCars.Count;
            Speed averageSpeed = activeCars > 0 ? speedSum / activeCars : new Speed(0);
            Time averageDelay = finishedCars > 0 ? delaySum / finishedCars : new Time(0);
            statistics.Update(allCars.Count, activeCars, finishedCars, carsWithZeroSpeed, averageSpeed, averageDelay);
        }

        private void GenerateCar(float activeNavigationRate, double probability = 1f)
        {
            if (probability < 1f && random.NextDouble() >= probability)
                return;

            Crossroad start = GetRandomCrossroad();
            Crossroad finish;
            do
                finish = GetRandomCrossroad();
            while (finish == start);
            Distance length = carLengthDistribution[random.Next(carLengthDistribution.Length)].Metres();
            bool active = random.NextDouble() < activeNavigationRate;
            INavigation navigation = centralNavigation.GetNavigation(start.Id, finish.Id, active);
            Car car = new Car(length, navigation, StatsCollector);
            stagedCars.Add(car);
            allCars.Add(car);
        }

        private Crossroad GetRandomCrossroad()
        {
            randomCrossroads.MoveNext();
            return randomCrossroads.Current;
        }

        private IEnumerable<Crossroad> GetRandomCrossroads()
        {
            List<Crossroad> crossroads = new List<Crossroad>();
            foreach (Crossroad crossroad in map.GetNodes())
                for (int i = 0; i < crossroad.CarSpawnRate; i++)
                    crossroads.Add(crossroad);
            int count = crossroads.Count;
            while (true)
                yield return crossroads[random.Next(count)];
        }

        #region statistics

        public interface IGlobalStatistics
        {
            public int CarsTotal { get; }
            public int CarsActive { get; }
            public int CarsFinished { get; }
            public int CarsWithZeroSpeed { get; }
            public Speed AverageSpeed { get; }
            public Time AverageDelay { get; }
            public IReadOnlyList<Timestamp<StatsData>> DataLog { get; }
        }

        public readonly struct StatsData
        {
            public readonly int carsTotal;
            public readonly int carsActive;
            public readonly int carsFinished;
            public readonly int carsWithZeroSpeed;
            public readonly Speed averageSpeed;
            public readonly Time averageDelay;

            public StatsData(int carsTotal, int carsActive, int carsFinished, int carsWithZeroSpeed,
                Speed averageSpeed, Time averageDelay)
            {
                this.carsTotal = carsTotal;
                this.carsActive = carsActive;
                this.carsFinished = carsFinished;
                this.carsWithZeroSpeed = carsWithZeroSpeed;
                this.averageSpeed = averageSpeed;
                this.averageDelay = averageDelay;
            }
        }

        private class GlobalStatistics : StatisticsBase, IGlobalStatistics
        {
            private StatsData currentData;
            private Item<List<Timestamp<StatsData>>> dataLog = new(DetailLevel.Medium, new());

            public int CarsTotal { get => currentData.carsTotal; }
            public int CarsActive { get => currentData.carsActive; }
            public int CarsFinished { get => currentData.carsFinished; }
            public int CarsWithZeroSpeed { get => currentData.carsWithZeroSpeed; }
            public Speed AverageSpeed { get => currentData.averageSpeed; }
            public Time AverageDelay { get => currentData.averageDelay; }
            public IReadOnlyList<Timestamp<StatsData>> DataLog { get => dataLog.Get(); }

            public GlobalStatistics(StatisticsCollector collector, IClock clock)
                : base(collector, typeof(Simulation), clock) { }

            public void Update(int carsTotal, int carsActive, int carsFinished, int carsWithZeroSpeed,
                Speed averageSpeed, Time averageDelay)
            {
                currentData = new StatsData(carsTotal, carsActive, carsFinished, carsWithZeroSpeed,
                    averageSpeed, averageDelay);
                dataLog.Get()?.Add(new Timestamp<StatsData>(clock.Time, currentData));
            }

            public override void Serialise(Utf8JsonWriter writer)
            {
                static void SerialiseData(Utf8JsonWriter writer, StatsData data)
                {
                    writer.WriteNumber("carsTotal", data.carsTotal);
                    writer.WriteNumber("carsActive", data.carsActive);
                    writer.WriteNumber("carsFinished", data.carsFinished);
                    writer.WriteNumber("carsWithZeroSpeed", data.carsWithZeroSpeed);
                    writer.WriteNumber("averageSpeed", data.averageSpeed);
                    writer.WriteNumber("averageDelay", data.averageDelay);
                }

                writer.WriteStartObject();

                writer.WriteNumber("endCarsTotal", CarsTotal);
                writer.WriteNumber("endCarsActive", CarsActive);
                writer.WriteNumber("endCarsFinished", CarsFinished);
                writer.WriteNumber("endCarsWithZeroSpeed", CarsWithZeroSpeed);
                writer.WriteNumber("endAverageSpeed", AverageSpeed);
                writer.WriteNumber("endAverageDelay", AverageDelay);
                SerialiseTimestampListItem(writer, dataLog, "dataLog", SerialiseData);

                writer.WriteEndObject();
            }
        }

        #endregion statistics
    }

    class SimulationSettings
    {
        // New cars per second
        private readonly float carSpawnFrequency;
        // Relative distribution through time (values from interval (0, 1])
        private readonly float[] carSpawnFrequencyDistribution;

        public Time Duration { get; }
        public Time TimeStep { get; set; }
        public float ActiveNavigationRate { get; }
        public StatisticsBase.DetailLevel StatisticsDetail { get; }

        public SimulationSettings(Time duration, float activeNavigationRate, StatisticsBase.DetailLevel statsDetail,
            float carSpawnFrequency, float[] carSpawnFrequencyDistribution)
        {
            Duration = duration;
            TimeStep = Simulation.MinTimeStep;
            ActiveNavigationRate = activeNavigationRate;
            StatisticsDetail = statsDetail;
            this.carSpawnFrequency = carSpawnFrequency;
            this.carSpawnFrequencyDistribution = carSpawnFrequencyDistribution;
        }

        /// <returns>New cars per second per crossroad.</returns>
        public float GetCarSpawnFrequency(Time simulationTime)
        {
            int index = carSpawnFrequencyDistribution.Length * (int)simulationTime / (int)Duration;
            return carSpawnFrequency * carSpawnFrequencyDistribution[index];
        }
    }
}
