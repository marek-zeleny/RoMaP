﻿using System;
using System.Diagnostics;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.ValueTypes
{
    static class Extensions
    {
        private static class Coefficients
        {
            public const int metres = Distance.precision;
            public const int millimetres = Distance.precision / 1000;
            public const int seconds = Time.precision;
            public const int milliseconds = Time.precision / 1000;
            public const int metresPerSecond = Speed.precision;
            public const int millimetresPerSecond = Speed.precision / 1000;
            public const int kilometresPerHour = Speed.precision * 10 / 36;
            public const int metresPerSecondPerSecond = Acceleration.precision;

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

        public static Distance Metres(this int value) => new Distance(value * Coefficients.metres);

        public static Distance Millimetres(this int value) => new Distance(value * Coefficients.millimetres);

        public static Time Seconds(this int value) => new Time(value * Coefficients.seconds);

        public static Time Milliseconds(this int value) => new Time(value * Coefficients.milliseconds);

        public static Speed MetresPerSecond(this int value) => new Speed(value * Coefficients.metresPerSecond);

        public static Speed MillimetresPerSecond(this int value) =>
            new Speed(value * Coefficients.millimetresPerSecond);

        public static Speed KilometresPerHour(this int value) => new Speed(value * Coefficients.kilometresPerHour);

        public static Acceleration MetresPerSecondPerSecond(this int value) =>
            new Acceleration(value * Coefficients.metresPerSecondPerSecond);


        public static Weight Weight(this Time value) => new Weight(value);
    }
}