using System;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.ValueTypes
{
    static class Extensions
    {
        public static Meters Meters(this int value) => new Meters(value);

        public static Seconds Seconds(this int value) => new Seconds(value);

        public static MetersPerSecond MetersPerSecond(this int value) => new MetersPerSecond(value);

        public static Weight Weight(this Seconds value) => new Weight(value);
    }
}