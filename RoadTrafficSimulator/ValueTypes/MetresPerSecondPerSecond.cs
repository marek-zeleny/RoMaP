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

        public static MetresPerSecond operator *(MetresPerSecondPerSecond mpss, Milliseconds ms)
        {
            MetresPerSecond mps = new MetresPerSecond(mpss.value * ms / 1000);
            // TODO: Figure out a way to make this operation safe
            Debug.Assert(mps != 0 || mpss == 0 || ms == 0);
            return mps;
        }

        public static MetresPerSecond operator *(Milliseconds ms, MetresPerSecondPerSecond mpss) => mpss * ms;

        public static Milliseconds operator /(MetresPerSecond mps, MetresPerSecondPerSecond mpss) =>
            new Milliseconds(1000 * mps / mpss.value);

        public static MetresPerSecondPerSecond operator *(MetresPerSecondPerSecond mpss, int i) =>
            new MetresPerSecondPerSecond(mpss.value * i);

        public static MetresPerSecondPerSecond operator *(int i, MetresPerSecondPerSecond mpss) => mpss * i;

        public static MetresPerSecondPerSecond operator /(MetresPerSecondPerSecond mpss, int i) =>
            new MetresPerSecondPerSecond(mpss.value / i);

        public override string ToString() => string.Format("{0:N0}mpss", value);
    }
}
