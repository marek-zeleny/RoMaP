using System;
using System.Collections.Generic;
using System.Linq;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    interface IClock
    {
        Seconds Time { get; }
    }

    class Simulation: IClock
    {
        private static readonly int[] carLengthDistribution = new int[] { 2, 3, 3, 3, 3, 4, 4, 10, 10, 15 };

        private Random random = new Random();
        private IEnumerator<Crossroad> randomCrossroads;
        private HashSet<Car> stagedCars;

        public Seconds Time { get; private set; }
        public Map Map { get; set; }
        public Statistics Statistics { get; private set; }

        public Simulation(Map map)
        {
            Map = map;
        }

        public enum InitializationResult { Ok, Error_MapIsNull, Error_NoMap, Error_InvalidCrossroad }

        public InitializationResult Initialize(out Crossroad invalidCrossroad)
        {
            invalidCrossroad = null;
            if (Map == null)
                return InitializationResult.Error_MapIsNull;
            if (Map.CrossroadCount == 0 || Map.RoadCount == 0)
                return InitializationResult.Error_NoMap;
            foreach (Crossroad c in Map.GetNodes())
                if (!c.Initialize())
                {
                    invalidCrossroad = c;
                    return InitializationResult.Error_InvalidCrossroad;
                }
            foreach (Road r in Map.GetEdges())
                r.Initialize();
            randomCrossroads = GetRandomCrossroads().GetEnumerator();
            stagedCars = new HashSet<Car>();
            Time = 0.Seconds();
            Statistics = new Statistics();
            return InitializationResult.Ok;
        }

        public void Simulate(Seconds duration, float newCarsPerHundredSecondsPerCrossroad) => Simulate(duration, newCarsPerHundredSecondsPerCrossroad, 1.Seconds());

        public void Simulate(Seconds duration, float newCarsPerHundredSecondsPerCrossroad, Seconds step)
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
            Meters length = carLengthDistribution[random.Next(carLengthDistribution.Length)].Meters();
            stagedCars.Add(new Car(length, Map, start, finish, this, DriveFinished));
        }

        public void Tick(Seconds time)
        {
            Time += time;
            HashSet<Car> releasedCars = new HashSet<Car>();
            foreach (Car c in stagedCars)
                if (c.Initialize())
                    releasedCars.Add(c);
            Statistics.AddCars(releasedCars.Count);
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
