using System;
using System.Diagnostics;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.ValueTypes
{
    static class Extensions
    {
        private static class Coefficients
        {
            public const int metres = ValueTypes.Millimetres.precision;
            public const int millimetres = ValueTypes.Millimetres.precision / 1000;
            public const int seconds = ValueTypes.Milliseconds.precision;
            public const int milliseconds = ValueTypes.Milliseconds.precision / 1000;
            public const int metresPerSecond = ValueTypes.MillimetresPerSecond.precision;
            public const int millimetresPerSecond = ValueTypes.MillimetresPerSecond.precision / 1000;
            public const int kilometresPerHour = ValueTypes.MillimetresPerSecond.precision * 10 / 36;
            public const int metresPerSecondPerSecond = ValueTypes.MetresPerSecondPerSecond.precision;

            static Coefficients()
            {
                Debug.Assert(metres > 0);
                Debug.Assert(millimetres > 0);
                Debug.Assert(seconds > 0);
                Debug.Assert(milliseconds > 0);
                Debug.Assert(metresPerSecond > 0);
                Debug.Assert(millimetresPerSecond > 0);
                Debug.Assert(kilometresPerHour > 0);
                Debug.Assert(metresPerSecondPerSecond > 0);
            }
        }

        public static Millimetres Metres(this int value) => new Millimetres(value * Coefficients.metres);

        public static Millimetres Millimetres(this int value) => new Millimetres(value * Coefficients.millimetres);

        public static Milliseconds Seconds(this int value) => new Milliseconds(value * Coefficients.seconds);

        public static Milliseconds Milliseconds(this int value) => new Milliseconds(value * Coefficients.milliseconds);

        public static MillimetresPerSecond MetresPerSecond(this int value) =>
            new MillimetresPerSecond(value * Coefficients.metresPerSecond);

        public static MillimetresPerSecond MillimetresPerSecond(this int value) =>
            new MillimetresPerSecond(value * Coefficients.millimetresPerSecond);

        public static MillimetresPerSecond KilometresPerHour(this int value) =>
            new MillimetresPerSecond(value * Coefficients.kilometresPerHour);

        public static MetresPerSecondPerSecond MetresPerSecondPerSecond(this int value) =>
            new MetresPerSecondPerSecond(value * Coefficients.metresPerSecondPerSecond);


        public static Weight Weight(this Milliseconds value) => new Weight(value);
    }
}