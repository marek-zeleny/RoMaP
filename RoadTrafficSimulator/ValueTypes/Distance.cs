﻿using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.ValueTypes
{
    readonly struct Distance
    {
        public const int precision = 1000;
        private const string unit = "mm";
        private const int convertToSpeedCoef = Speed.precision * Time.precision / precision;

        static Distance()
        {
            Debug.Assert(convertToSpeedCoef > 0);
        }

        private readonly int value;

        public Distance(int value)
        {
            this.value = value;
        }

        public static implicit operator int(Distance d) => d.value;

        public static explicit operator Distance(int i) => new Distance(i);

        public static Distance operator +(Distance d1, Distance d2) => new Distance(d1.value + d2.value);

        public static Distance operator -(Distance d) => new Distance(-d.value);

        public static Distance operator -(Distance d1, Distance d2) => new Distance(d1.value - d2.value);

        public static Speed operator /(Distance d, Time t) => new Speed(convertToSpeedCoef * d.value / (int)t);

        public static Distance operator *(Distance d, int i) => new Distance(d.value * i);

        public static Distance operator *(int i, Distance d) => d * i;

        public static Distance operator /(Distance d, int i) => new Distance(d.value / i);

        public override string ToString() => $"{value:N0}{unit}";
    }
}