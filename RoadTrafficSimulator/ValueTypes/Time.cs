using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    readonly struct Time : IComparable<Time>
    {
        public const int precision = 1000;
        private const string unit = "ms";
        private const int convertToDistanceCoef = Distance.precision / (precision * Speed.precision);
        private const int convertToDistanceCoefInverse = precision * Speed.precision / Distance.precision;

        private readonly int value;

        public Time(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Time t) => t.value;

        public static explicit operator Time(int i) => new Time(i);

        public static Time operator +(Time t1, Time t2) => new Time(t1.value + t2.value);

        public static Time operator -(Time t) => new Time(-t.value);

        public static Time operator -(Time t1, Time t2) => new Time(t1.value - t2.value);

        public static Time operator *(Time t, int i) => new Time(t.value * i);

        public static Time operator *(int i, Time t) => t * i;

        public static Time operator /(Time t, int i) => new Time(t.value / i);

        public static Distance operator *(Time t, Speed s)
        {
            // Statically evaluated
            if (convertToDistanceCoef > 0)
                return new Distance(t.value * (int)s * convertToDistanceCoef);
            else
                return new Distance(t.value * (int)s / convertToDistanceCoefInverse);
        }

        public static Distance operator *(Speed s, Time t) => t * s;

        public int CompareTo(Time other) => value.CompareTo(other.value);

        public override string ToString() => $"{value:N0}{unit}";
    }
}
