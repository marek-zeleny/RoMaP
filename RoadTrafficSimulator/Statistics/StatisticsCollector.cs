using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Statistics
{
    class StatisticsCollector
    {
        private Dictionary<Type, List<StatisticsBase>> data = new Dictionary<Type, List<StatisticsBase>>(2);

        private List<Car.Statistics> finishedCars = new List<Car.Statistics>();

        public int CarsFinished { get => finishedCars.Count; }
        public int CarsTotal { get; private set; }

        public void TrackStatistics(StatisticsBase statistics, Type owner)
        {
            if (!data.TryGetValue(owner, out var statList))
            {
                statList = new List<StatisticsBase>();
                data[owner] = statList;
            }
            statList.Add(statistics);
        }

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

        public Distance GetAverageDistance()
        {
            if (CarsFinished == 0)
                return 0.Metres();
            Distance totalDistance = new Distance(finishedCars.Sum(stats => stats.Distance));
            return totalDistance / CarsFinished;
        }

        public Time GetAverageDuration()
        {
            if (CarsFinished == 0)
                return 0.Milliseconds();
            Time totalDuration = new Time(finishedCars.Sum(stats => stats.Duration));
            return totalDuration / CarsFinished;
        }

        public Time GetAverageDelay()
        {
            if (CarsFinished == 0)
                return 0.Milliseconds();
            Time totalDelay = new Time(
                finishedCars.Sum(stats => stats.Duration - stats.ExpectedDuration));
            return totalDelay / CarsFinished;
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
                writer.WriteLine($"{from.x},{from.y},{to.x},{to.y},{(int)first.Time},{(int)s.Finish},{(int)s.ExpectedDuration},{(int)s.Distance}");
            }
            writer.Flush();
        }

        public void ExportJson(string path)
        {
            foreach (var (source, stats) in data)
            {
                string fileName = source.Name + "-statistics";
                string filePath = Path.ChangeExtension(Path.Combine(path, fileName), "json");
                using (Stream stream = File.OpenWrite(filePath)) // TODO: Find the best (and safest) way to open a file
                {
                    Utf8JsonWriter writer = new Utf8JsonWriter(stream); // TODO: Set options appropriately
                    foreach (var stat in stats)
                        stat.Serialise(writer);
                }
            }
        }
    }
}
