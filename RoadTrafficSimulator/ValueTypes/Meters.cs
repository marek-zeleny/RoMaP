using System;

namespace RoadTrafficSimulator.ValueTypes
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

        public static MetersPerSecond operator /(Meters m, Milliseconds ms) => new MetersPerSecond(1000 * m.value / ms);

        public static Meters operator *(Meters m, int i) => new Meters(m.value * i);

        public static Meters operator *(int i, Meters m) => m * i;

        public static Meters operator /(Meters m, int i) => new Meters(m.value / i);

        public static Meters operator *(Meters m, double d) => new Meters((int)(m.value * d));

        public static Meters operator *(double d, Meters m) => m * d;

        public static Meters operator /(Meters m, double d) => new Meters((int)(m.value / d));

        public override string ToString() => string.Format("{0:N0}m", value);
    }
}
