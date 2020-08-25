using System;

namespace RoadTrafficSimulator.Components
{
    struct Kilometers
    {
        private readonly double value;

        public Kilometers(double value)
        {
            this.value = value;
        }

        public static implicit operator double(Kilometers km) => km.value;

        public static explicit operator Kilometers(double d) => new Kilometers(d);

        public static KilometersPerHour operator /(Kilometers k, Hours h) => new KilometersPerHour((int)(k.value / h));

        public override string ToString() => value.ToString();
    }

    struct Hours
    {
        private readonly double value;

        public Hours(double value)
        {
            this.value = value;
        }

        public static implicit operator double(Hours h) => h.value;

        public static explicit operator Hours(double d) => new Hours(d);

        public static Kilometers operator *(Hours h, KilometersPerHour kph) => new Kilometers(h.value * kph);

        public static Kilometers operator *(KilometersPerHour kph, Hours h) => h * kph;

        public override string ToString() => value.ToString();
    }

    struct KilometersPerHour
    {
        private readonly int value;

        public KilometersPerHour(int value)
        {
            this.value = value;
        }

        public static implicit operator int(KilometersPerHour kph) => kph.value;

        public static explicit operator KilometersPerHour(int i) => new KilometersPerHour(i);

        public static Hours operator /(Kilometers k, KilometersPerHour kph) => new Hours(k / kph.value);

        public override string ToString() => value.ToString();
    }

    static class Extensions
    {
        public static Kilometers Kilometers(this double d) => new Kilometers(d);

        public static Hours Hours(this double d) => new Hours(d);

        public static KilometersPerHour KilometersPerHour(this int i) => new KilometersPerHour(i);
    }
}