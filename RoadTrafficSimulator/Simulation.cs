﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private HashSet<Car> stagedCars;

        public bool IsRunning { get => settings != null && Clock.Time < settings.Duration; }
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

            Statistics = new StatisticsCollector();
            foreach (Road r in this.map.GetEdges())
                r.Initialise(Statistics, clock);
            clock.Reset();
            centralNavigation = new CentralNavigation(this.map, clock);
            randomCrossroads = GetRandomCrossroads().GetEnumerator();
            stagedCars = new HashSet<Car>();
            return InitialisationResult.Ok;
        }

        public void StartSimulation(SimulationSettings settings, out Func<Time, bool> continueFunc)
        {
            Debug.Assert(map != null);
            this.settings = settings;
            continueFunc = ContinueSimulation;
        }

        private bool ContinueSimulation(Time duration)
        {
            Time end = clock.Time + duration;
            if (end > settings.Duration)
                end = settings.Duration;
            while (clock.Time < end)
            {
                double carsPerSecond = settings.GetCarSpawnFrequency(clock.Time) * map.CrossroadCount;
                // Not using TimeStep.ToSeconds() to achieve better precision
                double newCarProbability = carsPerSecond * settings.TimeStep / Time.precision;
                for (; newCarProbability > 0; newCarProbability--)
                    GenerateCar(settings.ActiveNavigationRate, newCarProbability);
                Tick(settings.TimeStep);
            }
            return clock.Time < settings.Duration;
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
            foreach (Road r in map.GetEdges())
                r.AfterTick();
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
        private readonly float[] carSpawnFrequencyDistribution;

        public Time Duration { get; }
        public Time TimeStep { get; set; }
        public float ActiveNavigationRate { get; }

        public SimulationSettings(Time duration, float activeNavigationRate, float[] carSpawnFrequencyDistribution)
        {
            Duration = duration;
            TimeStep = Simulation.MinTimeStep;
            ActiveNavigationRate = activeNavigationRate;
            this.carSpawnFrequencyDistribution = carSpawnFrequencyDistribution;
        }

        /// <returns>New cars per second per crossroad.</returns>
        public float GetCarSpawnFrequency(Time simulationTime)
        {
            int index = carSpawnFrequencyDistribution.Length * (int)simulationTime / (int)Duration;
            return carSpawnFrequencyDistribution[index];
        }
    }
}
