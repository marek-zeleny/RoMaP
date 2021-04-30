using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    struct MetresPerSecondPerSecond
    {
        private readonly int value;

        public MetresPerSecondPerSecond(int value)
        {
            this.value = value;
        }

        public static implicit operator int(MetresPerSecondPerSecond mpss) => mpss.value;

        public static explicit operator MetresPerSecondPerSecond(int i) => new MetresPerSecondPerSecond(i);

        public static MetresPerSecondPerSecond operator +(MetresPerSecondPerSecond mpss1,
            MetresPerSecondPerSecond mpss2) =>
            new MetresPerSecondPerSecond(mpss1.value + mpss2.value);

        public static MetresPerSecondPerSecond operator -(MetresPerSecondPerSecond mpss) =>
            new MetresPerSecondPerSecond(-mpss.value);

        public static MetresPerSecondPerSecond operator -(MetresPerSecondPerSecond mpss1,
            MetresPerSecondPerSecond mpss2) =>
            new MetresPerSecondPerSecond(mpss1.value - mpss2.value);

        public static MillimetresPerSecond operator *(MetresPerSecondPerSecond mpss, Milliseconds ms) =>
            new MillimetresPerSecond(mpss.value * (int)ms);

        public static MillimetresPerSecond operator *(Milliseconds ms, MetresPerSecondPerSecond mpss) => mpss * ms;

        public static Milliseconds operator /(MillimetresPerSecond mmps, MetresPerSecondPerSecond mpss) =>
            new Milliseconds(1000 * (int)mmps / mpss.value);

        public static MetresPerSecondPerSecond operator *(MetresPerSecondPerSecond mpss, int i) =>
            new MetresPerSecondPerSecond(mpss.value * i);

        public static MetresPerSecondPerSecond operator *(int i, MetresPerSecondPerSecond mpss) => mpss * i;

        public static MetresPerSecondPerSecond operator /(MetresPerSecondPerSecond mpss, int i) =>
            new MetresPerSecondPerSecond(mpss.value / i);

        public override string ToString() => string.Format("{0:N0}mpss", value);
    }
}
