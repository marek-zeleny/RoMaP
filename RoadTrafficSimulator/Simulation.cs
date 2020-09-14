﻿using System;
using System.Collections.Generic;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class Simulation
    {
        private static readonly int[] carLengthDistribution = new int[] { 2, 3, 3, 3, 3, 4, 4, 10, 10, 15 };

        private Random random = new Random();
        private IEnumerator<Crossroad> randomCrossroads;
        private HashSet<Car> stagedCars;

        public static Simulation Instance { get; } = new Simulation();

        public Seconds Time { get; private set; }
        public Map Map { get; private set; } = new Map();
        public Statistics Statistics { get; private set; }

        public bool Initialize()
        {
            foreach (Crossroad c in Map.GetNodes())
                if (!c.Initialize())
                    return false;
            randomCrossroads = GetRandomCrossroads().GetEnumerator();
            stagedCars = new HashSet<Car>();
            Time = 0.Seconds();
            Statistics = new Statistics();
            return true;
        }

        public void GenerateCar()
        {
            Crossroad start = GetRandomCrossroad();
            Crossroad finish;
            do
                finish = GetRandomCrossroad();
            while (finish == start);
            Meters length = carLengthDistribution[random.Next(carLengthDistribution.Length)].Meters();
            stagedCars.Add(new Car(length, Map, start, finish, Time, DriveFinished));
        }

        public void Tick(Seconds time)
        {
            HashSet<Car> releasedCars = new HashSet<Car>();
            foreach (Car c in stagedCars)
                if (c.Initialize())
                    releasedCars.Add(c);
            foreach (Car c in releasedCars)
                stagedCars.Remove(c);
            foreach (Crossroad c in Map.GetNodes())
                c.Tick(time);
            foreach (Road r in Map.GetEdges())
                r.Tick(time);
            Time += time;
        }

        public void DriveFinished(Car car)
        {
            Statistics.AddRecord(car, Time);
        }

        private Crossroad GetRandomCrossroad()
        {
            randomCrossroads.MoveNext();
            return randomCrossroads.Current;
        }

        private IEnumerable<Crossroad> GetRandomCrossroads()
        {
            List<Crossroad> crossroads = new List<Crossroad>((IEnumerable<Crossroad>)Map.GetNodes());
            int count = Map.CrossroadCount;
            while (true)
                yield return crossroads[random.Next(count)];
        }
    }
}
