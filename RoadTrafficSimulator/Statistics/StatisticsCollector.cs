using System;
using System.Collections.Generic;
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
        public void ExportCsv(string path)
        {
            string dirName = $"sim_stats_{DateTime.Now:yyyyMMdd_hhmmss}";
            string dirPath = Path.Combine(path, dirName);
            Directory.CreateDirectory(dirPath);
            foreach (var (source, stats) in data)
            {
                string subdirName = source.Name;
                string subdirPath = Path.Combine(dirPath, subdirName);

                // Function to create a file
                StreamWriter CreateFile(string fileName)
                {
                    // Create type-specific subdirectory
                    if (!Directory.Exists(subdirPath))
                        Directory.CreateDirectory(subdirPath);
                    string filePath = Path.Combine(subdirPath, fileName);
                    return new StreamWriter(filePath);
                }

                // Function to create a periodic data file
                StreamWriter CreatePeriodicDataFile(string dataId) => CreateFile($"periodic-data_{dataId}.csv");

                // Skip empty statistics
                if (stats.Count == 0)
                    continue;
                // Export constant-size data
                string header = stats[0].GetConstantDataHeader();
                if (header != null)
                {
                    using StreamWriter sw = CreateFile("constant-data.csv");
                    sw.WriteLine(header);
                    foreach (var stat in stats)
                        stat.SerialiseConstantData(sw);
                }
                // Export periodic data
                foreach (var stat in stats)
                    stat.SerialisePeriodicData(CreatePeriodicDataFile);
            }
        }
    }
}
