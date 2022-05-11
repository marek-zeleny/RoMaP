using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;
using DataStructures.Graphs;

namespace RoadTrafficSimulator
{
    /// <summary>
    /// Controls the simulation flow.
    /// </summary>
    class Simulation
    {
        /// <summary>
        /// Minimal allowed length of a simulation step.
        /// </summary>
        public static readonly Time MinTimeStep = 100.Milliseconds();

        /// <summary>
        /// Distribution from which lengths of cars will be randomly generated (in metres)
        /// </summary>
        private static readonly int[] carLengthDistribution = new int[] {
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4,   // short passenger cars (50 % -> 10x)
            5, 5, 5, 5, 5, 5,               // long passenger cars (30 % -> 6x)
            6, 6,                           // pick-ups (10 % -> 2x)
            14,                             // lorries (5 % -> 1x)
            8,                              // trailers (5 % -> 1x)
        };

        /// <summary>
        /// Measures the simulation time.
        /// </summary>
        private class SimulationClock : IClock
        {
            public Time Time { get; private set; }

            /// <summary>
            /// Moves the clock forward by a given interval.
            /// </summary>
            public void Tick(Time interval) => Time += interval;

            /// <summary>
            /// Resets the clock to time 0.
            /// </summary>
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

        /// <summary>
        /// <c>true</c> if the simulation is currently running, otherwise <c>false</c>
        /// </summary>
        public bool IsRunning { get => settings != null && Clock.Time < settings.Duration; }
        /// <summary>
        /// Simulation clock measuring current time
        /// </summary>
        public IClock Clock { get => clock; }
        /// <summary>
        /// Statistics collector for the simulation
        /// </summary>
        public StatisticsCollector StatsCollector { get; private set; }
        /// <summary>
        /// Global statistics measured for the simulation
        /// </summary>
        public IGlobalStatistics Statistics { get => statistics; }

        /// <summary>
        /// Creates a new simulation instance.
        /// </summary>
        public Simulation()
        {
            random = new Random();
            clock = new SimulationClock();
        }

        /// <summary>
        /// Result of simulation initialisation
        /// </summary>
        public enum InitialisationResult {
            /// <summary>
            /// Initialisation was successful
            /// </summary>
            Ok,
            /// <summary>
            /// Initialisation failed: the given map is <c>null</c>
            /// </summary>
            Error_MapIsNull,
            /// <summary>
            /// Initialisation failed: the given map is empty (has no roads nor crossroads)
            /// </summary>
            Error_NoMap,
            /// <summary>
            /// Initialisation failed: the given map is not strongly connected (there are some crossroads with no path
            /// between them)
            /// </summary>
            Error_NotConnected,
            /// <summary>
            /// Initialisation failed: some crossroad could not be initialised due to inconsistent state
            /// </summary>
            Error_InvalidCrossroad,
        }

        /// <summary>
        /// Initialise before running a new simulation.
        /// </summary>
        /// <param name="map">Map to run the simulation on</param>
        /// <param name="invalidCrossroad">
        /// If initialisation fails due to an invalid state of a crossroad, outputs that crossroad; otherwise the value
        /// is undefined
        /// </param>
        /// <returns>Result of the initialisation</returns>
        public InitialisationResult Initialise(Map map, out Crossroad invalidCrossroad)
        {
            clock.Reset();
            settings = null;
            invalidCrossroad = null;
            // Check if the map is non-empty
            if (map == null)
                return InitialisationResult.Error_MapIsNull;
            if (map.CrossroadCount == 0 || map.RoadCount == 0)
                return InitialisationResult.Error_NoMap;
            // Check if the map is strongly connected
            if (!map.IsStronglyConnected())
                return InitialisationResult.Error_NotConnected;
            // Initialise crossroads
            foreach (Crossroad c in map.GetNodes())
                if (!c.Initialise(Clock))
                {
                    invalidCrossroad = c;
                    return InitialisationResult.Error_InvalidCrossroad;
                }
            this.map = map;
            // Initialise supporting infrastructure
            StatsCollector = new StatisticsCollector();
            statistics = new GlobalStatistics(StatsCollector, Clock);
            allCars = new List<Car>();
            // Initialise roads
            foreach (Road r in this.map.GetEdges())
                r.Initialise(StatsCollector, Clock);
            // Initialise more supporting infrastructure
            centralNavigation = new CentralNavigation(this.map, Clock);
            randomCrossroads = GetRandomCrossroads().GetEnumerator();
            stagedCars = new HashSet<Car>();
            return InitialisationResult.Ok;
        }

        /// <summary>
        /// Starts a simulation on a currently initialised map with given simulation settings.
        /// </summary>
        /// <returns>Function to call for continuing the simulation for a given period of time</returns>
        public Func<Time, bool> StartSimulation(SimulationSettings settings)
        {
            Debug.Assert(map != null);
            this.settings = settings;
            StatisticsBase.detailSetting = settings.StatisticsDetail;
            return ContinueSimulation;
        }

        /// <summary>
        /// Continues simulating for a given period of time.
        /// </summary>
        /// <returns><c>true</c> if the simulation is not yet finished, otherwise <c>false</c></returns>
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

        /// <summary>
        /// Performs a simulation tick of a given length.
        /// </summary>
        private void Tick(Time timeStep)
        {
            // Generate new cars
            double carsPerSecond = settings.GetCarSpawnFrequency(Clock.Time) * map.CrossroadCount;
            // Not using TimeStep.ToSeconds() to achieve better precision
            double newCarProbability = carsPerSecond * timeStep / Time.precision;
            for (; newCarProbability > 0; newCarProbability--)
                GenerateCar(settings.ActiveNavigationRate, newCarProbability);
            // Release waiting cars
            HashSet<Car> releasedCars = new();
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
            Speed speedSum = new(0);
            Time delaySum = new(0);
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

        /// <summary>
        /// With a given probability, generates a new car with randomised parameters.
        /// </summary>
        /// <param name="activeNavigationRate">Percentage of actively navigated cars (number between 0 and 1)</param>
        /// <param name="probability">
        /// Probability of generating a new car; if greater than or equal to 1, a new car is guaranteed to be generated
        /// </param>
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
            INavigation navigation = centralNavigation.GetNavigation(start, finish, active);
            Car car = new(length, navigation, StatsCollector);
            stagedCars.Add(car);
            allCars.Add(car);
        }

        /// <summary>
        /// Gets a random crossroad with distribution given by car spawn rate of each crossroad.
        /// </summary>
        private Crossroad GetRandomCrossroad()
        {
            randomCrossroads.MoveNext();
            return randomCrossroads.Current;
        }

        /// <summary>
        /// Gets an enumerator generating random crossroads with distribution given by car spawn rate of each crossroad.
        /// </summary>
        private IEnumerable<Crossroad> GetRandomCrossroads()
        {
            List<Crossroad> crossroads = new();
            foreach (Crossroad crossroad in map.GetNodes())
                for (int i = 0; i < crossroad.CarSpawnRate; i++)
                    crossroads.Add(crossroad);
            int count = crossroads.Count;
            while (true)
                yield return crossroads[random.Next(count)];
        }

        #region statistics

        /// <summary>
        /// Interface describing global statistics collected by the simulation.
        /// </summary>
        public interface IGlobalStatistics
        {
            /// <summary>
            /// Total number of cars that entered the simulation
            /// </summary>
            public int CarsTotal { get; }
            /// <summary>
            /// Number of cars that are currently active (i.e. already begun their passage and haven't yet finished)
            /// </summary>
            public int CarsActive { get; }
            /// <summary>
            /// Number of cars that have already reached their destination
            /// </summary>
            public int CarsFinished { get; }
            /// <summary>
            /// Number of active cars that have zero speed at the moment
            /// </summary>
            public int CarsWithZeroSpeed { get; }
            /// <summary>
            /// Average speed of all active cars
            /// </summary>
            public Speed AverageSpeed { get; }
            /// <summary>
            /// Average delay of all finished cars
            /// </summary>
            public Time AverageDelay { get; }
            /// <summary>
            /// Periodically records global metrics of the simulation
            /// </summary>
            public IReadOnlyList<Timestamp<StatsData>> DataLog { get; }
        }

        /// <summary>
        /// Describes global statistical data of a running simulation at certain moment.
        /// </summary>
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
            private static StatsData AddStats(StatsData sd1, StatsData sd2) => new(
                sd1.carsTotal + sd2.carsTotal,
                sd1.carsActive + sd2.carsActive,
                sd1.carsFinished + sd2.carsFinished,
                sd1.carsWithZeroSpeed + sd2.carsWithZeroSpeed,
                sd1.averageSpeed + sd2.averageSpeed,
                sd1.averageDelay + sd2.averageDelay
                );

            private static StatsData DivideStats(StatsData sd, int n) => new(
                sd.carsTotal / n,
                sd.carsActive / n,
                sd.carsFinished / n,
                sd.carsWithZeroSpeed / n,
                sd.averageSpeed / n,
                sd.averageDelay / n
                );

            private StatsData currentData;
            private CumulativeListItem<StatsData> dataLog = new(DetailLevel.Medium, 1.Seconds(), AddStats, DivideStats);

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
                dataLog.Add(clock.Time, currentData);
            }

            public override string GetConstantDataHeader()
            {
                return null;
            }

            public override void SerialiseConstantData(TextWriter writer)
            {
                throw new InvalidOperationException();
            }

            public override void SerialisePeriodicData(Func<string, TextWriter> getWriterFunc)
            {
                if (!dataLog.IsActive)
                    return;
                dataLog.Flush(clock.Time);
                using TextWriter writer = getWriterFunc("global");
                // Write CSV header
                writer.WriteLine($"time ({Time.unit}),cars,active cars,finished cars,stationary cars," +
                    $"average speed ({Speed.unit}),average delay ({Time.unit})"
                    );
                // Write data
                foreach (var timestamp in dataLog.Get())
                {
                    writer.WriteLine(
                        $"{(int)timestamp.time},{timestamp.data.carsTotal},{timestamp.data.carsActive}," +
                        $"{timestamp.data.carsFinished},{timestamp.data.carsWithZeroSpeed}," +
                        $"{(int)timestamp.data.averageSpeed},{(int)timestamp.data.averageDelay}"
                        );
                }
            }
        }

        #endregion statistics
    }

    /// <summary>
    /// Contains settings and parameters of a simulation.
    /// </summary>
    class SimulationSettings
    {
        /// <summary>
        /// New cars per crossroad per second
        /// </summary>
        private readonly float carSpawnFrequency;
        /// <summary>
        /// Relative distribution through time (values from interval (0, 1])
        /// </summary>
        private readonly float[] carSpawnFrequencyDistribution;

        /// <summary>
        /// Duration of the simulation
        /// </summary>
        public Time Duration { get; }
        /// <summary>
        /// Length of one time step
        /// </summary>
        public Time TimeStep { get; set; }
        /// <summary>
        /// Percentage of cars with active navigation (value from interval [0, 1])
        /// </summary>
        public float ActiveNavigationRate { get; }
        /// <summary>
        /// Maximum detail level of statistics that should be collected
        /// </summary>
        public StatisticsBase.DetailLevel StatisticsDetail { get; }

        /// <summary>
        /// Creates new simulation settings with given parameters.
        /// </summary>
        /// <param name="duration">Duration of the simulation</param>
        /// <param name="activeNavigationRate">
        /// Percentage of cars with active navigation (value from interval [0, 1])
        /// </param>
        /// <param name="statsDetail">Maximum detail level of statistics that should be collected</param>
        /// <param name="carSpawnFrequency">New cars per crossroad per second</param>
        /// <param name="carSpawnFrequencyDistribution">
        /// Relative distribution through time (values from interval (0, 1])
        /// </param>
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

        /// <summary>
        /// Gets the number of new cars generated per second per crossroad at given simulation time.
        /// </summary>
        public float GetCarSpawnFrequency(Time simulationTime)
        {
            int index = Math.DivRem(carSpawnFrequencyDistribution.Length * (int)simulationTime, Duration, out int rem);
            float remRelative = rem / (float)Duration;
            float distributionCoef = carSpawnFrequencyDistribution[index] * remRelative
                + carSpawnFrequencyDistribution[index] * (1 - remRelative);
            return carSpawnFrequency * distributionCoef;
        }
    }
}
