using System;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Statistics
{
    abstract class StatisticsBase
    {
        protected IClock clock;

        protected StatisticsBase(IClock clock)
        {
            this.clock = clock;
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
