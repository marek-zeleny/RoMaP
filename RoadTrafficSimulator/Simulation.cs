using System;
using System.Collections.Generic;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator
{
    class Simulation
    {
        private static readonly int[] carLengthDistribution = new int[] { 2, 3, 3, 3, 3, 4, 4, 10, 10, 15 };

        private class SimulationClock : IClock
        {
            public Time Time { get; private set; }

            public void Tick(Time interval) => Time += interval;

            public void Reset() => Time = new Time();
        }

        private Random random = new Random();
        private SimulationClock clock;
        private CentralNavigation centralNavigation;
        private IEnumerator<Crossroad> randomCrossroads;
        private HashSet<Car> stagedCars;

        public IClock Clock { get => clock; }
        public Map Map { get; set; }
        public StatisticsCollector Statistics { get; private set; }

        public Simulation(Map map)
        {
            Map = map;
            clock = new SimulationClock();
        }

        public enum InitialisationResult { Ok, Error_MapIsNull, Error_NoMap, Error_InvalidCrossroad }

        public InitialisationResult Initialise(out Crossroad invalidCrossroad)
        {
            invalidCrossroad = null;
            if (Map == null)
                return InitialisationResult.Error_MapIsNull;
            if (Map.CrossroadCount == 0 || Map.RoadCount == 0)
                return InitialisationResult.Error_NoMap;
            foreach (Crossroad c in Map.GetNodes())
                if (!c.Initialise())
                {
                    invalidCrossroad = c;
                    return InitialisationResult.Error_InvalidCrossroad;
                }
            foreach (Road r in Map.GetEdges())
                r.Initialise(clock);
            clock.Reset();
            centralNavigation = new CentralNavigation(Map, clock);
            randomCrossroads = GetRandomCrossroads().GetEnumerator();
            stagedCars = new HashSet<Car>();
            Statistics = new StatisticsCollector();
            return InitialisationResult.Ok;
        }

        public void Simulate(SimulationSettings settings)
        {
            while (clock.Time < settings.Duration)
            {
                double carsPerSecond = settings.GetCarSpawnRate(clock.Time) * Map.CrossroadCount;
                double newCarProbability = carsPerSecond * settings.TimeStep / Time.precision;
                for (; newCarProbability > 0; newCarProbability--)
                    GenerateCar(settings.ActiveNavigationRate, newCarProbability);
                Tick(settings.TimeStep);
            }
        }

        public void GenerateCar(float activeNavigationRate, double probability = 1f)
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
            stagedCars.Add(new Car(length, navigation, DriveFinished));
            Statistics.AddCars(1);
        }

        public void Tick(Time time)
        {
            clock.Tick(time);
            HashSet<Car> releasedCars = new HashSet<Car>();
            foreach (Car c in stagedCars)
                if (c.Initialise())
                    releasedCars.Add(c);
            foreach (Car c in releasedCars)
                stagedCars.Remove(c);
            foreach (Crossroad c in Map.GetNodes())
                c.Tick(time);
            foreach (Road r in Map.GetEdges())
                r.Tick(time);
        }

        private void DriveFinished(Car.Statistics statistics)
        {
            Statistics.AddFinishedCar(statistics);
        }

        private Crossroad GetRandomCrossroad()
        {
            randomCrossroads.MoveNext();
            return randomCrossroads.Current;
        }

        private IEnumerable<Crossroad> GetRandomCrossroads()
        {
            List<Crossroad> crossroads = new List<Crossroad>();
            foreach (Crossroad crossroad in Map.GetNodes())
                for (int i = 0; i < crossroad.CarSpawnRate; i++)
                    crossroads.Add(crossroad);
            int count = crossroads.Count;
            while (true)
                yield return crossroads[random.Next(count)];
        }
    }

    class SimulationSettings
    {
        // New cars per second
        private float[] carSpawnRateDistribution;

        public Time Duration { get; }
        public Time TimeStep { get; set; }
        public float ActiveNavigationRate { get; }

        /// <returns>New cars per second per crossroad.</returns>
        public float GetCarSpawnRate(Time simulationTime)
        {
            int index = carSpawnRateDistribution.Length * (int)simulationTime / (int)Duration;
            return carSpawnRateDistribution[index];
        }
    }
}
