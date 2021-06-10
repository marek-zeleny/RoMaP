using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace RoadTrafficSimulator.Statistics
{
    class StatisticsCollector
    {
        private Dictionary<Type, List<StatisticsBase>> data = new(2);
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

        public void ExportJson(string path)
        {
            foreach (var (source, stats) in data)
            {
                string fileName = source.Name + "-statistics";
                string filePath = Path.ChangeExtension(Path.Combine(path, fileName), "json");
                using (Stream stream = File.OpenWrite(filePath)) // TODO: Find the best (and safest) way to open a file
                {
                    Utf8JsonWriter writer = new(stream); // TODO: Set options appropriately
                    writer.WriteStartObject();
                    writer.WriteStartArray($"{source.Name}Statistics");
                    foreach (var stat in stats)
                        stat.Serialise(writer);
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                    writer.Flush();
                }
            }
        }
    }
}
