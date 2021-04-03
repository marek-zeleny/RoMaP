using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Statistics
{
    class StatisticsCollector
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

        public void ExportCSV(TextWriter writer)
        {
            writer.WriteLine("From(x),From(y),To(x),To(y),Start(s),Finish(s),ExpectedDuration(s),Distance(m)");
            foreach (var s in finishedCars)
            {
                var first = s.RoadLog[0];
                var last = s.RoadLog[s.RoadLog.Count - 1];
                var from = first.Road.FromNode.Id;
                var to = last.Road.ToNode.Id;
                writer.WriteLine($"{from.x},{from.y},{to.x},{to.y},{(int)first.Time},{(int)s.End},{(int)s.ExpectedDuration},{(int)s.Distance}");
            }
            writer.Flush();
        }
    }
}
