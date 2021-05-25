using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        private readonly Random random;
        private readonly SimulationClock clock;
        private Map map;
        private CentralNavigation centralNavigation;
        private IEnumerator<Crossroad> randomCrossroads;
        private HashSet<Car> stagedCars;

        public IClock Clock { get => clock; }
        public StatisticsCollector Statistics { get; private set; }

        public Simulation()
        {
            random = new Random();
            clock = new SimulationClock();
        }

        public enum InitialisationResult { Ok, Error_MapIsNull, Error_NoMap, Error_InvalidCrossroad }

        public InitialisationResult Initialise(Map map, out Crossroad invalidCrossroad)
        {
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

            Statistics = new StatisticsCollector();
            foreach (Road r in this.map.GetEdges())
                r.Initialise(Statistics, clock);
            clock.Reset();
            centralNavigation = new CentralNavigation(this.map, clock);
            randomCrossroads = GetRandomCrossroads().GetEnumerator();
            stagedCars = new HashSet<Car>();
            return InitialisationResult.Ok;
        }

        public void Simulate(SimulationSettings settings)
        {
            Debug.Assert(map != null);
            while (clock.Time < settings.Duration)
            {
                double carsPerSecond = settings.GetCarSpawnRate(clock.Time) * map.CrossroadCount;
                // Not using TimeStep.ToSeconds() to achieve better precision
                double newCarProbability = carsPerSecond * settings.TimeStep / Time.precision;
                for (; newCarProbability > 0; newCarProbability--)
                    GenerateCar(settings.ActiveNavigationRate, newCarProbability);
                Tick(settings.TimeStep);
            }
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
            stagedCars.Add(new Car(length, navigation, Statistics));
        }

        private void Tick(Time time)
        {
            clock.Tick(time);
            HashSet<Car> releasedCars = new HashSet<Car>();
            foreach (Car c in stagedCars)
                if (c.Initialise())
                    releasedCars.Add(c);
            foreach (Car c in releasedCars)
                stagedCars.Remove(c);
            foreach (Crossroad c in map.GetNodes())
                c.Tick(time);
            foreach (Road r in map.GetEdges())
                r.Tick(time);
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
    }

    class SimulationSettings
    {
        // New cars per second
        private readonly float[] carSpawnRateDistribution;

        public Time Duration { get; }
        public Time TimeStep { get; set; }
        public float ActiveNavigationRate { get; }

        public SimulationSettings(Time duration, float activeNavigationRate, float[] carSpawnRateDistribution)
        {
            Duration = duration;
            TimeStep = 300.Milliseconds();
            ActiveNavigationRate = activeNavigationRate;
            this.carSpawnRateDistribution = carSpawnRateDistribution;
        }

        /// <returns>New cars per second per crossroad.</returns>
        public float GetCarSpawnRate(Time simulationTime)
        {
            int index = carSpawnRateDistribution.Length * (int)simulationTime / (int)Duration;
            return carSpawnRateDistribution[index];
        }
    }
}
