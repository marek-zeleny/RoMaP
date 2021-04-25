using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct MetresPerSecond
    {
        private readonly int value;

        public MetresPerSecond(int value)
        {
            this.value = value;
        }

        public static implicit operator int(MetresPerSecond mps) => mps.value;

        public static explicit operator MetresPerSecond(int i) => new MetresPerSecond(i);

        public static MetresPerSecond operator +(MetresPerSecond mps1, MetresPerSecond mps2) =>
            new MetresPerSecond(mps1.value + mps2.value);

        public static MetresPerSecond operator -(MetresPerSecond mps) => new MetresPerSecond(-mps.value);

        public static MetresPerSecond operator -(MetresPerSecond mps1, MetresPerSecond mps2) =>
            new MetresPerSecond(mps1.value - mps2.value);

        public static Milliseconds operator /(Millimetres mm, MetresPerSecond mps) => new Milliseconds(mm / mps.value);

        public static MetresPerSecondPerSecond operator /(MetresPerSecond mps, Milliseconds ms) =>
            new MetresPerSecondPerSecond(1000 * mps.value / ms);

        public static MetresPerSecond operator *(MetresPerSecond mps, int i) => new MetresPerSecond(mps.value * i);

        public static MetresPerSecond operator *(int i, MetresPerSecond mps) => mps * i;

        public static MetresPerSecond operator /(MetresPerSecond mps, int i) => new MetresPerSecond(mps.value / i);

        public override string ToString() => string.Format("{0:N0}mps", value);
    }
}
