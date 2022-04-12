using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    /// <summary>
    /// Represents a speed (distance over time) value.
    /// </summary>
    readonly struct Speed : IComparable<Speed>
    {
        public const int precision = 1000;
        private const string unit = "mmps";
        private const int convertToTimeCoef = Time.precision * precision / Distance.precision;
        private const int convertToTimeCoefInverse = Distance.precision / (Time.precision * precision);
        private const int convertToAccelerationCoef = Acceleration.precision * Time.precision / precision;
        private const int convertToAccelerationCoefInverse = precision / (Acceleration.precision * Time.precision);

        private readonly int value;

        public Speed(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Speed s) => s.value;

        public static explicit operator Speed(int i) => new(i);

        public static Speed operator +(Speed s1, Speed s2) => new(s1.value + s2.value);

        public static Speed operator -(Speed s) => new(-s.value);

        public static Speed operator -(Speed s1, Speed s2) => new(s1.value - s2.value);

        public static Speed operator *(Speed s, int i) => new(s.value * i);

        public static Speed operator *(int i, Speed s) => s * i;

        public static Speed operator /(Speed s, int i) => new(s.value / i);

        public static Time operator /(Distance d, Speed s)
        {
            // Statically evaluated
            if (convertToTimeCoef > 0)
                return new Time(convertToTimeCoef * (int)d / s.value);
            else
                return new Time((int)d / (s.value * convertToTimeCoefInverse));
        }

        public static Acceleration operator /(Speed s, Time t)
        {
            // Statically evaluated
            if (convertToAccelerationCoef > 0)
                return new Acceleration(convertToAccelerationCoef * s.value / (int)t);
            else
                return new Acceleration(s.value / ((int)t * convertToAccelerationCoefInverse));
        }

        public int CompareTo(Speed other) => value.CompareTo(other.value);

        public override string ToString() => $"{value:N0}{unit}";
    }
}
