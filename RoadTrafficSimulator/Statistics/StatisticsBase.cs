using System;

using RoadTrafficSimulator.ValueTypes;

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

        protected StatisticsBase(IClock clock)
        {
            this.clock = clock;
        }

        public struct Item<T>
        {
            private T data;
            public DetailLevel Detail { get; }

            public Item(DetailLevel detail, T data = default)
            {
                Detail = detail;
                if (detail < detailSettings)
                    data = default;
                this.data = data;
            }

            public T Get()
            {
                if (Detail >= detailSettings)
                    return data;
                else
                    return default;
            }

            public void Set(T value)
            {
                if (Detail >= detailSettings)
                    data = value;
            }

            public static implicit operator T(Item<T> item) => item.Get();
        }

        public struct Timestamp<T>
        {
            public readonly Seconds time;
            public readonly T data;

            public Timestamp(Seconds time, T data)
            {
                this.time = time;
                this.data = data;
            }

            public override string ToString() => $"{time}: {data}";
        }
    }
}
