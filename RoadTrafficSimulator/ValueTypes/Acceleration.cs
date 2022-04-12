using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    /// <summary>
    /// Represents an acceleration (speed over time) value.
    /// </summary>
    readonly struct Acceleration : IComparable<Acceleration>
    {
        public const int precision = 1;
        private const string unit = "mpss";
        private const int convertToSpeedCoef = Speed.precision / (Time.precision * precision);
        private const int convertToSpeedCoefInverse = Time.precision * precision / Speed.precision;
        private const int convertToTimeCoef = Time.precision * precision / Speed.precision;
        private const int convertToTimeCoefInverse = Speed.precision / (Time.precision * precision);

        private readonly int value;

        public Acceleration(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Acceleration a) => a.value;

        public static explicit operator Acceleration(int i) => new(i);

        public static Acceleration operator +(Acceleration a1, Acceleration a2) => new(a1.value + a2.value);

        public static Acceleration operator -(Acceleration a) => new(-a.value);

        public static Acceleration operator -(Acceleration a1, Acceleration a2) => new(a1.value - a2.value);

        public static Acceleration operator *(Acceleration a, int i) => new(a.value * i);

        public static Acceleration operator *(int i, Acceleration a) => a * i;

        public static Acceleration operator /(Acceleration a, int i) => new(a.value / i);

        public static Speed operator *(Acceleration a, Time t)
        {
            // Statically evaluated
            if (convertToSpeedCoef > 0)
                return new Speed(convertToSpeedCoef * a.value * (int)t);
            else
                return new Speed(a.value * (int)t / convertToSpeedCoefInverse);
        }

        public static Speed operator *(Time t, Acceleration a) => a * t;

        public static Time operator /(Speed s, Acceleration a)
        {
            // Statically evaluated
            if (convertToTimeCoef > 0)
                return new Time(convertToTimeCoef * (int)s / a.value);
            else
                return new Time((int)s / (a.value * convertToTimeCoefInverse));
        }

        public int CompareTo(Acceleration other) => value.CompareTo(other.value);

        public override string ToString() => $"{value:N0}{unit}";
    }
}
