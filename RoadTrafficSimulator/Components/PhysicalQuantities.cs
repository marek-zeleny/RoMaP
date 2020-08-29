using System;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    struct Meters
    {
        private readonly int value;

        public Meters(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Meters m) => m.value;

        public static explicit operator Meters(int i) => new Meters(i);

        public static Meters operator +(Meters m1, Meters m2) => new Meters(m1.value + m2.value);

        public static Meters operator -(Meters m) => new Meters(-m.value);

        public static Meters operator -(Meters m1, Meters m2) => new Meters(m1.value - m2.value);

        public static MetersPerSecond operator /(Meters m, Seconds s) => new MetersPerSecond(m.value / s);

        public override string ToString() => value.ToString();
    }

    struct Seconds
    {
        private readonly int value;

        public Seconds(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Seconds s) => s.value;

        public static explicit operator Seconds(int i) => new Seconds(i);

        public static Seconds operator +(Seconds s1, Seconds s2) => new Seconds(s1.value + s2.value);

        public static Seconds operator -(Seconds s) => new Seconds(-s.value);

        public static Seconds operator -(Seconds s1, Seconds s2) => new Seconds(s1.value - s2.value);

        public static Meters operator *(Seconds s, MetersPerSecond mps) => new Meters(s.value * mps);

        public static Meters operator *(MetersPerSecond mps, Seconds s) => s * mps;

        public override string ToString() => value.ToString();
    }

    struct MetersPerSecond
    {
        private readonly int value;

        public MetersPerSecond(int value)
        {
            this.value = value;
        }

        public static implicit operator int(MetersPerSecond mps) => mps.value;

        public static explicit operator MetersPerSecond(int i) => new MetersPerSecond(i);

        public static MetersPerSecond operator +(MetersPerSecond mps1, MetersPerSecond mps2) => new MetersPerSecond(mps1.value + mps2.value);

        public static MetersPerSecond operator -(MetersPerSecond mps) => new MetersPerSecond(-mps.value);

        public static MetersPerSecond operator -(MetersPerSecond mps1, MetersPerSecond mps2) => new MetersPerSecond(mps1.value - mps2.value);

        public static Seconds operator /(Meters m, MetersPerSecond mps) => new Seconds(m / mps.value);

        public override string ToString() => value.ToString();
    }

    static class PhysicalQuantitiesExtensions
    {
        public static Meters Meters(this int value) => new Meters(value);

        public static Seconds Seconds(this int value) => new Seconds(value);

        public static MetersPerSecond MetersPerSecond(this int value) => new MetersPerSecond(value);

        public static Weight Weight(this Seconds value) => new Weight(value);
    }
}