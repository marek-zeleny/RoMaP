using System;
using System.Collections.Generic;
using System.Text;

namespace RoadTrafficSimulator.ValueTypes
{
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
}
