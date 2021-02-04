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
        private List<Car.Statistics> records = new List<Car.Statistics>();

        public int RecordCount { get => records.Count; }

        public void AddRecord(Car.Statistics record)
        {
            if (record.Duration < record.ExpectedDuration)
                throw new Exception($"Duration less than expected duration: {record.Duration} < {record.ExpectedDuration}");
            records.Add(record);
        }

        public Meters GetAverageDistance()
        {
            if (RecordCount == 0)
                return 0.Meters();
            int totalDistance = records.Sum(record => record.Distance);
            return (totalDistance / RecordCount).Meters();
        }

        public Seconds GetAverageDuration()
        {
            if (RecordCount == 0)
                return 0.Seconds();
            int totalDuration = records.Sum(record => record.Duration);
            return (totalDuration / RecordCount).Seconds();
        }

        public Seconds GetAverageDelay()
        {
            if (RecordCount == 0)
                return 0.Seconds();
            int totalDelay = records.Sum(record => record.Duration - record.ExpectedDuration);
            return (totalDelay / RecordCount).Seconds();
        }

        public void ExportCSV(StringWriter writer)
        {
            writer.WriteLine("From;To;Start;Finish;ExpectedDuration;Distance");
            foreach (var r in records)
            {
                var first = r.RoadLog[0];
                var last = r.RoadLog[r.RoadLog.Count - 1];
                writer.WriteLine($"{first.Road.FromNode};{last.Road.ToNode};{first.Time};{r.End};{r.ExpectedDuration};{r.Distance}");
            }
            writer.Flush();
        }
    }
}
