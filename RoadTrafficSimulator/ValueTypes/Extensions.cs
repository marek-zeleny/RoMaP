using System;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.ValueTypes
{
    static class Extensions
    {
        public static Meters Meters(this int value) => new Meters(value);

        public static Milliseconds Milliseconds(this int value) => new Milliseconds(value);

        public static Milliseconds Seconds(this int value) => new Milliseconds(value * 1000);

        public static MetersPerSecond MetersPerSecond(this int value) => new MetersPerSecond(value);

        public static MetersPerSecondPerSecond MetersPerSecondPerSecond(this int value) =>
            new MetersPerSecondPerSecond(value);


        public static Weight Weight(this Milliseconds value) => new Weight(value);
    }
}