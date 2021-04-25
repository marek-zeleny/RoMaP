﻿using System;

namespace RoadTrafficSimulator.ValueTypes
{
    struct Milliseconds
    {
        private readonly int value;

        public Milliseconds(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Milliseconds ms) => ms.value;

        public static explicit operator Milliseconds(int i) => new Milliseconds(i);

        public static Milliseconds operator +(Milliseconds ms1, Milliseconds ms2) => new Milliseconds(ms1.value + ms2.value);

        public static Milliseconds operator -(Milliseconds ms) => new Milliseconds(-ms.value);

        public static Milliseconds operator -(Milliseconds ms1, Milliseconds ms2) => new Milliseconds(ms1.value - ms2.value);

        public static Millimetres operator *(Milliseconds ms, MetresPerSecond mps) => new Millimetres(ms.value * mps);

        public static Millimetres operator *(MetresPerSecond mps, Milliseconds ms) => ms * mps;

        public static Milliseconds operator *(Milliseconds ms, int i) => new Milliseconds(ms.value * i);

        public static Milliseconds operator *(int i, Milliseconds ms) => ms * i;

        public static Milliseconds operator /(Milliseconds ms, int i) => new Milliseconds(ms.value / i);

        public override string ToString() => string.Format("{0:N0}ms", value);
    }
}
