using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RoadTrafficSimulator.Statistics
{
    abstract class StatisticsBase
    {
        public enum DetailLevel
        {
            Low,
            Medium,
            High
        };

        public static DetailLevel detailSettings = DetailLevel.Medium;

        protected IClock clock;

        protected StatisticsBase(StatisticsCollector collector, Type owner, IClock clock)
        {
            this.clock = clock;
            collector.TrackStatistics(this, owner);
        }

        public abstract void Serialise(Utf8JsonWriter writer);

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

        protected struct Item<T>
        {
            private T data;
            public DetailLevel Detail { get; }
            public bool IsActive { get => Detail >= detailSettings; }

            public Item(DetailLevel detail, T data = default)
            {
                Detail = detail;
                if (detail < detailSettings)
                    data = default;
                this.data = data;
            }

            public T Get()
            {
                return IsActive ? data : default;
            }

            public void Set(T value)
            {
                if (Detail >= detailSettings)
                    data = value;
            }

            public static implicit operator T(Item<T> item) => item.Get();

            public override string ToString() => IsActive ? data.ToString() : string.Empty;
        }

        #endregion structures
    }
}
