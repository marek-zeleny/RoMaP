using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Statistics
{
    /// <summary>
    /// Abstract class serving as a base for statistics-measuring classes.
    /// </summary>
    abstract class StatisticsBase
    {
        public enum DetailLevel
        {
            Low,
            Medium,
            High
        };

        /// <summary>
        /// Current setting of the maximum detail level for which statistics are measured
        /// </summary>
        public static DetailLevel detailSetting = DetailLevel.Medium;

        protected IClock clock;

        /// <summary>
        /// Creates a new statistics class bound to a given collector, measuring statistics of a given owner type.
        /// </summary>
        /// <param name="clock">Global clock providing current time of the simulation</param>
        protected StatisticsBase(StatisticsCollector collector, Type owner, IClock clock)
        {
            this.clock = clock;
            collector.TrackStatistics(this, owner);
        }

        /// <summary>
        /// Gets the header of a CSV file for constant-size data collected by this class.
        /// </summary>
        /// <remarks>
        /// This function should be instance-independent (type-specific), but it cannot be static, because static
        /// functions cannot be overridden.
        /// </remarks>
        /// <returns>
        /// String containing a CSV header, or <c>null</c> if the class does not collect any constant-size data
        /// </returns>
        public abstract string GetConstantDataHeader();

        /// <summary>
        /// Serialises collected constant-size data into a given writer in a CSV format.
        /// </summary>
        /// <exception cref="InvalidOperationException">The class does not collect constant-size data.</exception>
        public abstract void SerialiseConstantData(TextWriter writer);

        /// <summary>
        /// Serialises collected periodic data into a text writer in a CSV format.
        /// </summary>
        /// <param name="getWriterFunc">
        /// Function for obtaining the text writer given an instance ID.
        /// If the class does not collect periodic data, the function is never called.
        /// </param>
        public abstract void SerialisePeriodicData(Func<string, TextWriter> getWriterFunc);

        #region serialisation_helpers

        protected static void SerialiseStringItem<T>(Utf8JsonWriter writer, Item<T> item, string name)
        {
            if (item.IsActive)
                writer.WriteString(name, item.Get().ToString());
        }

        protected static void SerialiseIntItem<T>(Utf8JsonWriter writer, Item<T> item, string name,
            Func<T, int> convertFunc)
        {
            if (item.IsActive)
                writer.WriteNumber(name, convertFunc(item));
        }

        protected static void SerialiseTimestampListItem<T>(Utf8JsonWriter writer, Item<List<Timestamp<T>>> list,
            string listName, string dataName)
        {
            void serialiser(Utf8JsonWriter writer, T data) => writer.WriteString(dataName, data.ToString());
            SerialiseTimestampListItem(writer, list, listName, serialiser);
        }

        protected static void SerialiseTimestampListItem<T>(Utf8JsonWriter writer, Item<List<Timestamp<T>>> list,
            string listName, Action<Utf8JsonWriter, T> timestampDataSerialiser)
        {
            if (list.IsActive)
            {
                writer.WriteStartArray(listName);
                foreach (var timestamp in list.Get())
                    SerialiseTimestamp(writer, timestamp, timestampDataSerialiser);
                writer.WriteEndArray();
            }
        }

        protected static void SerialiseTimestamp<T>(Utf8JsonWriter writer, Timestamp<T> timestamp, string dataName)
        {
            void serialiser(Utf8JsonWriter writer, T data) => writer.WriteString(dataName, data.ToString());
            SerialiseTimestamp(writer, timestamp, serialiser);
        }

        protected static void SerialiseTimestamp<T>(Utf8JsonWriter writer, Timestamp<T> timestamp,
            Action<Utf8JsonWriter, T> dataSerialiser)
        {
            writer.WriteStartObject();
            writer.WriteNumber("time", timestamp.time);
            dataSerialiser(writer, timestamp.data);
            writer.WriteEndObject();
        }

        #endregion serialisation_helpers

        #region structures

        /// <summary>
        /// Represents a single statistical datum.
        /// </summary>
        /// <typeparam name="T">Type of the stored data</typeparam>
        protected struct Item<T>
        {
            private T data;
            /// <summary>
            /// Detail level of the statistic; if <see cref="detailSetting"/> is lower, the statistic is not measured
            /// </summary>
            public DetailLevel Detail { get; }
            /// <summary>
            /// <c>true</c> if the statistic is measured (based on its detail level), otherwise <c>false</c>
            /// </summary>
            public bool IsActive { get => detailSetting >= Detail; }

            /// <summary>
            /// Creates a new statistical item, optionally with an initial value.
            /// </summary>
            /// <param name="detail">
            /// Detail level of the statistic; if <see cref="detailSetting"/> is lower, the statistic is not measured
            /// </param>
            public Item(DetailLevel detail, T data = default)
            {
                Detail = detail;
                //if (detailSetting < detail)
                //    data = default;
                // TODO: this makes logically sense, but it would require to initialise some items before each simulation; consider this
                this.data = data;
            }

            /// <summary>
            /// Gets the data stored in the item.
            /// </summary>
            /// <returns>Stored data if the item is active, otherwise <c>default</c></returns>
            public T Get()
            {
                return IsActive ? data : default;
            }

            /// <summary>
            /// Stores a given value in the item. If the item is not active, the new value is discarded.
            /// </summary>
            public void Set(T value)
            {
                if (IsActive)
                    data = value;
            }

            public static implicit operator T(Item<T> item) => item.Get();

            public override string ToString() => IsActive ? data.ToString() : string.Empty;
        }

        /// <summary>
        /// Represents a list of timestamped statistical data.
        /// </summary>
        /// <typeparam name="T">Type of the stored data</typeparam>
        protected struct ListItem<T>
        {
            private readonly List<Timestamp<T>> data;
            /// <summary>
            /// Detail level of the statistic; if <see cref="detailSetting"/> is lower, the statistic is not measured
            /// </summary>
            public DetailLevel Detail { get; }
            /// <summary>
            /// <c>true</c> if the statistic is measured (based on its detail level), otherwise <c>false</c>
            /// </summary>
            public bool IsActive { get => detailSetting >= Detail; }

            /// <summary>
            /// Creates a new list of timestamped data.
            /// </summary>
            /// <param name="detail">
            /// Detail level of the statistic; if <see cref="detailSetting"/> is lower, the statistic is not measured
            /// </param>
            public ListItem(DetailLevel detail)
            {
                Detail = detail;
                //if (detailSetting < detail)
                //    data = null;
                // TODO: this makes logically sense, but it would require to initialise some items before each simulation; consider this
                data = new List<Timestamp<T>>();
            }

            /// <summary>
            /// Gets the list of timestamped data stored in the item.
            /// </summary>
            /// <returns>Stored data if the item is active, otherwise <c>null</c></returns>
            public IReadOnlyList<Timestamp<T>> Get()
            {
                return IsActive ? data : default;
            }

            /// <summary>
            /// Adds a given timestamped value into the list. If the item is not active, the value is discarded.
            /// </summary>
            public void Add(Time time, T value)
            {
                if (IsActive)
                    data.Add(new Timestamp<T>(time, value));
            }

            public override string ToString() => IsActive ? $"ListItem: {data.Count}" : string.Empty;
        }

        /// <summary>
        /// Represents a list of timestamped statistical data that only stores data accumulated and aggregated over
        /// some period of time.
        /// </summary>
        /// <typeparam name="T">Type of the stored data</typeparam>
        protected struct CumulativeListItem<T>
        {
            private readonly ListItem<T> data;
            private readonly Time accumInterval;
            private readonly Func<T, T, T> accumulate;
            private readonly Func<T, int, T> aggregate;

            private T accumulator;
            private int accumCount;
            private int lastAccumIdx;

            /// <summary>
            /// Detail level of the statistic; if <see cref="detailSetting"/> is lower, the statistic is not measured
            /// </summary>
            public DetailLevel Detail { get => data.Detail; }
            /// <summary>
            /// <c>true</c> if the statistic is measured (based on its detail level), otherwise <c>false</c>
            /// </summary>
            public bool IsActive { get => data.IsActive; }

            /// <summary>
            /// Creates a new list accumulating timestamped statistical data.
            /// </summary>
            /// <param name="detail">
            /// Detail level of the statistic; if <see cref="detailSetting"/> is lower, the statistic is not measured
            /// </param>
            /// <param name="accumInterval">Time interval over which the data is accumulated</param>
            /// <param name="accumulate">Function for accumulating data</param>
            /// <param name="aggregate">Function for aggregating a stored result from accumulated data</param>
            public CumulativeListItem(DetailLevel detail, Time accumInterval,
                Func<T, T, T> accumulate, Func<T, int, T> aggregate)
            {
                data = new ListItem<T>(detail);
                this.accumInterval = accumInterval;
                this.accumulate = accumulate;
                this.aggregate = aggregate;
                accumulator = default;
                accumCount = 0;
                lastAccumIdx = 0;
            }

            /// <summary>
            /// Gets the list of timestamped data stored in the item.
            /// </summary>
            /// <returns>Stored data if the item is active, otherwise <c>null</c></returns>
            public IReadOnlyList<Timestamp<T>> Get() => data.Get();

            /// <summary>
            /// Accumulates a given timestamped value. If the item is not active, the value is discarded.
            /// If the accumulation interval has been exceeded since the last accumulation, the aggregates a value from
            /// the accumulated data and stores it into the list.
            /// </summary>
            public void Add(Time time, T value)
            {
                if (!IsActive)
                    return;
                int accumIdx = (int)time / accumInterval;
                // No data accumulated yet
                if (accumCount == 0)
                {
                    accumulator = value;
                    lastAccumIdx = accumIdx;
                    accumCount = 1;
                    return;
                }
                // Aggregating and adding to data
                if (accumIdx > lastAccumIdx)
                {
                    data.Add(accumIdx * accumInterval, aggregate(accumulator, accumCount));
                    accumulator = value;
                    accumCount = 1;
                    lastAccumIdx = accumIdx;
                }
                // Accumulating
                else
                {
                    accumulator = accumulate(accumulator, value);
                    accumCount++;
                }
            }

            /// <summary>
            /// Flushes accumulated data, aggregates a value and stores it into the list.
            /// </summary>
            public void Flush(Time time)
            {
                if (!IsActive || accumCount == 0)
                    return;
                int accumIdx = (int)time / accumInterval;
                if (accumIdx <= lastAccumIdx)
                    accumIdx = lastAccumIdx + 1;
                data.Add(accumIdx * accumInterval, aggregate(accumulator, accumCount));
                accumulator = default;
                accumCount = 0;
                lastAccumIdx = accumIdx;
            }

            public override string ToString() => data.ToString();
        }

        #endregion structures
    }
}
