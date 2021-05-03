using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    readonly struct MillimetresPerSecond
    {
        public const int precision = 1000;
        private const string unit = "mmps";
        private const int convertToTimeCoef = Milliseconds.precision * precision / Millimetres.precision;

        static MillimetresPerSecond()
        {
            Debug.Assert(convertToTimeCoef > 0);
        }

        private readonly int value;

        public MillimetresPerSecond(int value)
        {
            this.value = value;
        }

        public static implicit operator int(MillimetresPerSecond mmps) => mmps.value;

        public static explicit operator MillimetresPerSecond(int i) => new MillimetresPerSecond(i);

        public static MillimetresPerSecond operator +(MillimetresPerSecond mmps1, MillimetresPerSecond mmps2) =>
            new MillimetresPerSecond(mmps1.value + mmps2.value);

        public static MillimetresPerSecond operator -(MillimetresPerSecond mmps) =>
            new MillimetresPerSecond(-mmps.value);

        public static MillimetresPerSecond operator -(MillimetresPerSecond mmps1, MillimetresPerSecond mmps2) =>
            new MillimetresPerSecond(mmps1.value - mmps2.value);

        public static Milliseconds operator /(Millimetres mm, MillimetresPerSecond mmps) =>
            new Milliseconds(convertToTimeCoef * (int)mm / mmps.value);

        public static MetresPerSecondPerSecond operator /(MillimetresPerSecond mmps, Milliseconds ms) =>
            new MetresPerSecondPerSecond(mmps.value / (int)ms);

        public static MillimetresPerSecond operator *(MillimetresPerSecond mmps, int i) =>
            new MillimetresPerSecond(mmps.value * i);

        public static MillimetresPerSecond operator *(int i, MillimetresPerSecond mmps) => mmps * i;

        public static MillimetresPerSecond operator /(MillimetresPerSecond mmps, int i) =>
            new MillimetresPerSecond(mmps.value / i);

        public override string ToString() => $"{value:N0}{unit}";
    }
}
