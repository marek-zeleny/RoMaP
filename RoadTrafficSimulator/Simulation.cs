﻿using System;
using System.Collections.Generic;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator
{
    interface IClock
    {
        Milliseconds Time { get; }
    }

    class Simulation: IClock
    {
        private static readonly int[] carLengthDistribution = new int[] { 2, 3, 3, 3, 3, 4, 4, 10, 10, 15 };

        private Random random = new Random();
        private IEnumerator<Crossroad> randomCrossroads;
        private HashSet<Car> stagedCars;

        public Milliseconds Time { get; private set; }
        public Map Map { get; set; }
        public StatisticsCollector Statistics { get; private set; }

        public Simulation(Map map)
        {
            Map = map;
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
                r.Initialise();
            randomCrossroads = GetRandomCrossroads().GetEnumerator();
            stagedCars = new HashSet<Car>();
            Time = 0.Milliseconds();
            Statistics = new StatisticsCollector();
            return InitialisationResult.Ok;
        }

        public void Simulate(Milliseconds duration, float newCarsPerHundredSecondsPerCrossroad) => Simulate(duration, newCarsPerHundredSecondsPerCrossroad, 1.Seconds());

        public void Simulate(Milliseconds duration, float newCarsPerHundredSecondsPerCrossroad, Milliseconds step)
        {
            int newCarsPerHundredSeconds = (int)(newCarsPerHundredSecondsPerCrossroad * Map.CrossroadCount);
            while (Time < duration)
            {
                if (Time % 100 < step)
                    for (int i = 0; i < newCarsPerHundredSeconds; i++)
                        GenerateCar();
                Tick(step);
            }
        }

        public void GenerateCar()
        {
            Crossroad start = GetRandomCrossroad();
            Crossroad finish;
            do
                finish = GetRandomCrossroad();
            while (finish == start);
            Millimetres length = carLengthDistribution[random.Next(carLengthDistribution.Length)].Metres();
            stagedCars.Add(new Car(length, Map, start, finish, this, DriveFinished));
            Statistics.AddCars();
        }

        public void Tick(Milliseconds time)
        {
            Time += time;
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
}
