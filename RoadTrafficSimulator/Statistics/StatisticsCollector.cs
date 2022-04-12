using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace RoadTrafficSimulator.Statistics
{
    /// <summary>
    /// Collects statistics from the simulation and provides methods for exporting them.
    /// </summary>
    class StatisticsCollector
    {
        private Dictionary<Type, List<StatisticsBase>> data = new(2);

        /// <summary>
        /// Registers a statistics-measuring class to track.
        /// </summary>
        /// <param name="statistics">Statistics class to collect</param>
        /// <param name="owner">Type of the owner of the statistics</param>
        public void TrackStatistics(StatisticsBase statistics, Type owner)
        {
            if (!data.TryGetValue(owner, out var statList))
            {
                statList = new List<StatisticsBase>();
                data[owner] = statList;
            }
            statList.Add(statistics);
        }

        /// <summary>
        /// Exports all collected statistics to a given path in the file system.
        /// </summary>
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
