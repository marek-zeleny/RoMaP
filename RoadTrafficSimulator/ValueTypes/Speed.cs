using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    readonly struct Speed
    {
        public const int precision = 1000;
        private const string unit = "mmps";
        private const int convertToTimeCoef = Time.precision * precision / Distance.precision;

        static Speed()
        {
            Debug.Assert(convertToTimeCoef > 0);
        }

        private readonly int value;

        public Speed(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Speed s) => s.value;

        public static explicit operator Speed(int i) => new Speed(i);

        public static Speed operator +(Speed s1, Speed s2) => new Speed(s1.value + s2.value);

        public static Speed operator -(Speed s) => new Speed(-s.value);

        public static Speed operator -(Speed s1, Speed s2) => new Speed(s1.value - s2.value);

        public static Time operator /(Distance d, Speed s) => new Time(convertToTimeCoef * (int)d / s.value);

        public static Acceleration operator /(Speed s, Time t) => new Acceleration(s.value / (int)t);

        public static Speed operator *(Speed s, int i) => new Speed(s.value * i);

        public static Speed operator *(int i, Speed s) => s * i;

        public static Speed operator /(Speed s, int i) => new Speed(s.value / i);

        public override string ToString() => $"{value:N0}{unit}";
    }
}
