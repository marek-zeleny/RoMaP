using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    /// <summary>
    /// Represents a distance value.
    /// </summary>
    readonly struct Distance : IComparable<Distance>
    {
        /// <summary>
        /// Precision coefficient of distance relative to SI units (metres).
        /// </summary>
        /// <remarks>
        /// In order to change the physical quantity's precision, only change this constant and the string unit,
        /// the rest is done automatically.
        /// </remarks>
        public const int precision = 1000;
        public const string unit = "mm";

        private const int convertToSpeedCoef = Speed.precision * Time.precision / precision;
        private const int convertToSpeedCoefInverse = precision / (Speed.precision * Time.precision);

        private readonly int value;

        public Distance(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Distance d) => d.value;

        public static explicit operator Distance(int i) => new(i);

        public static Distance operator +(Distance d1, Distance d2) => new(d1.value + d2.value);

        public static Distance operator -(Distance d) => new(-d.value);

        public static Distance operator -(Distance d1, Distance d2) => new(d1.value - d2.value);

        public static Distance operator *(Distance d, int i) => new(d.value * i);

        public static Distance operator *(int i, Distance d) => d * i;

        public static Distance operator /(Distance d, int i) => new(d.value / i);

        public static Speed operator /(Distance d, Time t)
        {
            // Statically evaluated
            if (convertToSpeedCoef > 0)
                return new Speed(convertToSpeedCoef * d.value / (int)t);
            else
                return new Speed(d.value / ((int)t * convertToSpeedCoefInverse));
        }

        public int CompareTo(Distance other) => value.CompareTo(other.value);

        public override string ToString() => $"{value:N0}{unit}";
    }
}
