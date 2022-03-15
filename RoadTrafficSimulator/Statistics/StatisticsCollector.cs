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
            string dirName = $"sim_stats_{DateTime.Now:yyyyMMdd_hhmmss}";
            string dirPath = Path.Combine(path, dirName);
            Directory.CreateDirectory(dirPath);
            JsonWriterOptions options = new()
            {
                Indented = true,
            };
            foreach (var (source, stats) in data)
            {
                string fileName = source.Name;
                string filePath = Path.ChangeExtension(Path.Combine(dirPath, fileName), "json");
                using (Stream stream = File.OpenWrite(filePath))
                {
                    Utf8JsonWriter writer = new(stream, options);

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
