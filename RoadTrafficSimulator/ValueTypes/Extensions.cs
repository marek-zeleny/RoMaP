using System;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.ValueTypes
{
    static class Extensions
    {
        public static Millimetres Millimetres(this int value) => new Millimetres(value);

        public static Millimetres Metres(this int value) => new Millimetres(value * 1000);

        public static Milliseconds Milliseconds(this int value) => new Milliseconds(value);

        public static Milliseconds Seconds(this int value) => new Milliseconds(value * 1000);

        public static MillimetresPerSecond MillimetresPerSecond(this int value) => new MillimetresPerSecond(value);

        public static MillimetresPerSecond MetresPerSecond(this int value) => new MillimetresPerSecond(value * 1000);

        public static MillimetresPerSecond KilometresPerHour(this int value) =>
            new MillimetresPerSecond(value * 10_000 / 36);

        public static MetresPerSecondPerSecond MetresPerSecondPerSecond(this int value) =>
            new MetresPerSecondPerSecond(value);


        public static Weight Weight(this Milliseconds value) => new Weight(value);
    }
}