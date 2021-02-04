using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class Statistics
    {
        private List<Car.Statistics> finishedCars = new List<Car.Statistics>();

        public int CarsFinished { get => finishedCars.Count; }
        public int CarsTotal { get; private set; }

        public void AddCars(int count = 1)
        {
            CarsTotal+= count;
        }

        public void AddFinishedCar(Car.Statistics stats)
        {
            if (stats.Duration < stats.ExpectedDuration)
                throw new Exception($"Duration less than expected duration: {stats.Duration} < {stats.ExpectedDuration}");
            finishedCars.Add(stats);
        }

        public Meters GetAverageDistance()
        {
            if (CarsFinished == 0)
                return 0.Meters();
            int totalDistance = finishedCars.Sum(stats => stats.Distance);
            return (totalDistance / CarsFinished).Meters();
        }

        public Seconds GetAverageDuration()
        {
            if (CarsFinished == 0)
                return 0.Seconds();
            int totalDuration = finishedCars.Sum(stats => stats.Duration);
            return (totalDuration / CarsFinished).Seconds();
        }

        public Seconds GetAverageDelay()
        {
            if (CarsFinished == 0)
                return 0.Seconds();
            int totalDelay = finishedCars.Sum(stats => stats.Duration - stats.ExpectedDuration);
            return (totalDelay / CarsFinished).Seconds();
        }

        public void ExportCSV(StringWriter writer)
        {
            writer.WriteLine("From;To;Start;Finish;ExpectedDuration;Distance");
            foreach (var s in finishedCars)
            {
                var first = s.RoadLog[0];
                var last = s.RoadLog[s.RoadLog.Count - 1];
                writer.WriteLine($"{first.Road.FromNode};{last.Road.ToNode};{first.Time};{s.End};{s.ExpectedDuration};{s.Distance}");
            }
            writer.Flush();
        }
    }
}
