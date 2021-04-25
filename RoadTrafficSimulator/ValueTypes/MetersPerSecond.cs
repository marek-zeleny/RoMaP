using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct MetersPerSecond
    {
        private readonly int value;

        public MetersPerSecond(int value)
        {
            this.value = value;
        }

        public static implicit operator int(MetersPerSecond mps) => mps.value;

        public static explicit operator MetersPerSecond(int i) => new MetersPerSecond(i);

        public static MetersPerSecond operator +(MetersPerSecond mps1, MetersPerSecond mps2) =>
            new MetersPerSecond(mps1.value + mps2.value);

        public static MetersPerSecond operator -(MetersPerSecond mps) => new MetersPerSecond(-mps.value);

        public static MetersPerSecond operator -(MetersPerSecond mps1, MetersPerSecond mps2) =>
            new MetersPerSecond(mps1.value - mps2.value);

        public static Seconds operator /(Meters m, MetersPerSecond mps) => new Seconds(m / mps.value);

        public static MetersPerSecondPerSecond operator /(MetersPerSecond mps, Seconds s) =>
            new MetersPerSecondPerSecond(mps.value / s);

        public static MetersPerSecond operator *(MetersPerSecond mps, int i) => new MetersPerSecond(mps.value * i);

        public static MetersPerSecond operator *(int i, MetersPerSecond mps) => mps * i;

        public static MetersPerSecond operator /(MetersPerSecond mps, int i) => new MetersPerSecond(mps.value / i);

        public override string ToString() => string.Format("{0:N0}mps", value);
    }
}
