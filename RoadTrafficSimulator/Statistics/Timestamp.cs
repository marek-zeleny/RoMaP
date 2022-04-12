using System;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Statistics
{
    /// <summary>
    /// Represents statistical datum marked by a timestamp.
    /// </summary>
    /// <typeparam name="T">Type of the statistical data</typeparam>
    struct Timestamp<T>
    {
        public readonly Time time;
        public readonly T data;

        public Timestamp(Time time, T data)
        {
            this.time = time;
            this.data = data;
        }

        public override string ToString() => $"{time}: {data}";
    }
}
