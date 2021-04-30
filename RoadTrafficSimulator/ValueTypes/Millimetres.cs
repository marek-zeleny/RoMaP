using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct Millimetres
    {
        private readonly int value;

        public Millimetres(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Millimetres mm) => mm.value;

        public static explicit operator Millimetres(int i) => new Millimetres(i);

        public static Millimetres operator +(Millimetres mm1, Millimetres mm2) =>
            new Millimetres(mm1.value + mm2.value);

        public static Millimetres operator -(Millimetres mm) => new Millimetres(-mm.value);

        public static Millimetres operator -(Millimetres mm1, Millimetres mm2) =>
            new Millimetres(mm1.value - mm2.value);

        public static MillimetresPerSecond operator /(Millimetres mm, Milliseconds ms) =>
            new MillimetresPerSecond(1000 * mm.value / (int)ms);

        public static Millimetres operator *(Millimetres mm, int i) => new Millimetres(mm.value * i);

        public static Millimetres operator *(int i, Millimetres mm) => mm * i;

        public static Millimetres operator /(Millimetres mm, int i) => new Millimetres(mm.value / i);

        public override string ToString() => string.Format("{0:N0}mm", value);
    }
}
