using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    readonly struct Acceleration
    {
        public const int precision = 1;
        private const string unit = "mpss";
        private const int convertToSpeedCoef = Speed.precision / Time.precision * precision;
        private const int convertToTimeCoef = Time.precision * precision / Speed.precision;

        static Acceleration()
        {
            Debug.Assert(convertToSpeedCoef > 0);
            Debug.Assert(convertToTimeCoef > 0);
        }

        private readonly int value;

        public Acceleration(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Acceleration a) => a.value;

        public static explicit operator Acceleration(int i) => new Acceleration(i);

        public static Acceleration operator +(Acceleration a1, Acceleration a2) =>
            new Acceleration(a1.value + a2.value);

        public static Acceleration operator -(Acceleration a) => new Acceleration(-a.value);

        public static Acceleration operator -(Acceleration a1, Acceleration a2) =>
            new Acceleration(a1.value - a2.value);

        public static Speed operator *(Acceleration a, Time t) => new Speed(convertToSpeedCoef * a.value * (int)t);

        public static Speed operator *(Time t, Acceleration a) => a * t;

        public static Time operator /(Speed s, Acceleration a) => new Time(convertToTimeCoef * (int)s / a.value);

        public static Acceleration operator *(Acceleration a, int i) => new Acceleration(a.value * i);

        public static Acceleration operator *(int i, Acceleration a) => a * i;

        public static Acceleration operator /(Acceleration a, int i) => new Acceleration(a.value / i);

        public override string ToString() => $"{value:N0}{unit}";
    }
}
