using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct MillimetresPerSecond
    {
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
            new Milliseconds(1000 * (int)mm / mmps.value);

        public static MetresPerSecondPerSecond operator /(MillimetresPerSecond mmps, Milliseconds ms) =>
            new MetresPerSecondPerSecond(mmps.value / (int)ms);

        public static MillimetresPerSecond operator *(MillimetresPerSecond mmps, int i) =>
            new MillimetresPerSecond(mmps.value * i);

        public static MillimetresPerSecond operator *(int i, MillimetresPerSecond mmps) => mmps * i;

        public static MillimetresPerSecond operator /(MillimetresPerSecond mmps, int i) =>
            new MillimetresPerSecond(mmps.value / i);

        public override string ToString() => string.Format("{0:N0}mmps", value);
    }
}
