using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct MetersPerSecondPerSecond
    {
        private readonly int value;

        public MetersPerSecondPerSecond(int value)
        {
            this.value = value;
        }

        public static implicit operator int(MetersPerSecondPerSecond mpss) => mpss.value;

        public static explicit operator MetersPerSecondPerSecond(int i) => new MetersPerSecondPerSecond(i);

        public static MetersPerSecondPerSecond operator +(MetersPerSecondPerSecond mpss1,
            MetersPerSecondPerSecond mpss2) =>
            new MetersPerSecondPerSecond(mpss1.value + mpss2.value);

        public static MetersPerSecondPerSecond operator -(MetersPerSecondPerSecond mpss) =>
            new MetersPerSecondPerSecond(-mpss.value);

        public static MetersPerSecondPerSecond operator -(MetersPerSecondPerSecond mpss1,
            MetersPerSecondPerSecond mpss2) =>
            new MetersPerSecondPerSecond(mpss1.value - mpss2.value);

        public static MetersPerSecond operator *(MetersPerSecondPerSecond mpss, Milliseconds ms) =>
            new MetersPerSecond(mpss.value * ms / 1000);

        public static MetersPerSecond operator *(Milliseconds ms, MetersPerSecondPerSecond mpss) => mpss * ms;

        public static Milliseconds operator /(MetersPerSecond mps, MetersPerSecondPerSecond mpss) =>
            new Milliseconds(1000 * mps / mpss.value);

        public static MetersPerSecondPerSecond operator *(MetersPerSecondPerSecond mpss, int i) =>
            new MetersPerSecondPerSecond(mpss.value * i);

        public static MetersPerSecondPerSecond operator *(int i, MetersPerSecondPerSecond mpss) => mpss * i;

        public static MetersPerSecondPerSecond operator /(MetersPerSecondPerSecond mpss, int i) =>
            new MetersPerSecondPerSecond(mpss.value / i);

        public override string ToString() => string.Format("{0:N0}mpss", value);
    }
}
