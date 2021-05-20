using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    readonly struct Speed
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

        public static explicit operator Speed(int i) => new Speed(i);

        public static Speed operator +(Speed s1, Speed s2) => new Speed(s1.value + s2.value);

        public static Speed operator -(Speed s) => new Speed(-s.value);

        public static Speed operator -(Speed s1, Speed s2) => new Speed(s1.value - s2.value);

        public static Speed operator *(Speed s, int i) => new Speed(s.value * i);

        public static Speed operator *(int i, Speed s) => s * i;

        public static Speed operator /(Speed s, int i) => new Speed(s.value / i);

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

        public override string ToString() => $"{value:N0}{unit}";
    }
}
