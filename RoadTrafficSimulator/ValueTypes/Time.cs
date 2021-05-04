using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    readonly struct Time
    {
        public const int precision = 1000;
        private const string unit = "ms";
        private const int convertToDistanceCoefInverse = precision * Speed.precision / Distance.precision;

        static Time()
        {
            Debug.Assert(convertToDistanceCoefInverse > 0);
        }

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

        public static Distance operator *(Time t, Speed s) =>
            new Distance(t.value * (int)s / convertToDistanceCoefInverse);

        public static Distance operator *(Speed s, Time t) => t * s;

        public static Time operator *(Time t, int i) => new Time(t.value * i);

        public static Time operator *(int i, Time t) => t * i;

        public static Time operator /(Time t, int i) => new Time(t.value / i);

        public override string ToString() => $"{value:N0}{unit}";
    }
}
