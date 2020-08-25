using DataStructures.Graphs;
using System;

namespace RoadTrafficSimulator.Components
{
    struct Meters
    {
        private readonly uint value;

        public Meters(uint value)
        {
            this.value = value;
        }

        public static implicit operator uint(Meters m) => m.value;

        public static explicit operator Meters(uint i) => new Meters(i);

        public static Meters operator +(Meters m1, Meters m2) => new Meters(m1.value + m2.value);

        public static Meters operator -(Meters m1, Meters m2)
        {
            if (m1 < m2)
                throw new ArithmeticException(string.Format("The resulting value of {0} cannot be negative.", nameof(Meters)));
            return new Meters(m1.value - m2.value);
        }

        public static MetersPerSecond operator /(Meters m, Seconds s) => new MetersPerSecond(m.value / s);

        public override string ToString() => value.ToString();
    }

    struct Seconds
    {
        private readonly uint value;

        public Seconds(uint value)
        {
            this.value = value;
        }

        public static implicit operator uint(Seconds s) => s.value;

        public static explicit operator Seconds(uint i) => new Seconds(i);

        public static Seconds operator +(Seconds s1, Seconds s2) => new Seconds(s1.value + s2.value);

        public static Seconds operator -(Seconds s1, Seconds s2)
        {
            if (s1 < s2)
                throw new ArithmeticException(string.Format("The resulting value of {0} cannot be negative.", nameof(Seconds)));
            return new Seconds(s1.value - s2.value);
        }

        public static Meters operator *(Seconds s, MetersPerSecond mps) => new Meters(s.value * mps);

        public static Meters operator *(MetersPerSecond mps, Seconds s) => s * mps;

        public override string ToString() => value.ToString();
    }

    struct MetersPerSecond
    {
        private readonly uint value;

        public MetersPerSecond(uint value)
        {
            this.value = value;
        }

        public static implicit operator uint(MetersPerSecond mps) => mps.value;

        public static explicit operator MetersPerSecond(uint i) => new MetersPerSecond(i);

        public static MetersPerSecond operator +(MetersPerSecond mps1, MetersPerSecond mps2) => new MetersPerSecond(mps1.value + mps2.value);

        public static MetersPerSecond operator -(MetersPerSecond mps1, MetersPerSecond mps2)
        {
            if (mps1 < mps2)
                throw new ArithmeticException(string.Format("The resulting value of {0} cannot be negative.", nameof(MetersPerSecond)));
            return new MetersPerSecond(mps1.value - mps2.value);
        }

        public static Seconds operator /(Meters m, MetersPerSecond mps) => new Seconds(m / mps.value);

        public override string ToString() => value.ToString();
    }

    static class PhysicalQuantitiesExtensions
    {
        public static Meters Meters(this int value)
        {
            if (value < 0)
                throw new ArgumentException(string.Format("The value of {0} cannot be negative.", nameof(Meters)), nameof(value));
            return new Meters((uint)value);
        }

        public static Seconds Seconds(this int value)
        {
            if (value < 0)
                throw new ArgumentException(string.Format("The value of {0} cannot be negative.", nameof(Seconds)), nameof(value));
            return new Seconds((uint)value);
        }

        public static MetersPerSecond MetersPerSecond(this int value)
        {
            if (value < 0)
                throw new ArgumentException(string.Format("The value of {0} cannot be negative.", nameof(MetersPerSecond)), nameof(value));
            return new MetersPerSecond((uint)value);
        }

        public static Weight Weight(this Seconds value) => new Weight(value);
    }
}