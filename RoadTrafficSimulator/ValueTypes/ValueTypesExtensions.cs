using System;
using System.Diagnostics;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.ValueTypes
{
    static class ValueTypesExtensions
    {
        private static class Coefficients
        {
            // Distance
            public const int metres = Distance.precision;
            public const int millimetres = metres / 1000;
            // Time
            public const int seconds = Time.precision;
            public const int milliseconds = seconds / 1000;
            public const int minutes = seconds * 60;
            public const int hours = minutes * 60;
            public const int days = hours * 24;
            // Speed
            public const int metresPerSecond = Speed.precision;
            public const int millimetresPerSecond = metresPerSecond / 1000;
            public const int kilometresPerHour = metresPerSecond * 10 / 36;
            // Acceleration
            public const int metresPerSecondPerSecond = Acceleration.precision;

            static Coefficients()
            {
                Debug.Assert(metres > 0);
                Debug.Assert(millimetres > 0);
                Debug.Assert(seconds > 0);
                Debug.Assert(milliseconds > 0);
                Debug.Assert(minutes > 0);
                Debug.Assert(hours > 0);
                Debug.Assert(days > 0);
                Debug.Assert(metresPerSecond > 0);
                Debug.Assert(millimetresPerSecond > 0);
                Debug.Assert(kilometresPerHour > 0);
                Debug.Assert(metresPerSecondPerSecond > 0);
            }
        }

        #region from_int_to_quant

        // Distance
        public static Distance Millimetres(this int value) => new Distance(value * Coefficients.millimetres);

        public static Distance Metres(this int value) => new Distance(value * Coefficients.metres);

        // Time
        public static Time Milliseconds(this int value) => new Time(value * Coefficients.milliseconds);

        public static Time Seconds(this int value) => new Time(value * Coefficients.seconds);

        public static Time Minutes(this int value) => new Time(value * Coefficients.minutes);

        public static Time Hours(this int value) => new Time(value * Coefficients.hours);

        public static Time Days(this int value) => new Time(value * Coefficients.days);

        // Speed
        public static Speed MillimetresPerSecond(this int value) =>
            new Speed(value * Coefficients.millimetresPerSecond);

        public static Speed MetresPerSecond(this int value) => new Speed(value * Coefficients.metresPerSecond);

        public static Speed KilometresPerHour(this int value) => new Speed(value * Coefficients.kilometresPerHour);

        // Acceleration
        public static Acceleration MetresPerSecondPerSecond(this int value) =>
            new Acceleration(value * Coefficients.metresPerSecondPerSecond);

        #endregion from_int_to_quant

        #region from_quant_to_int

        // Distance
        public static int ToMillimetres(this Distance d) => (int)d / Coefficients.millimetres;

        public static int ToMetres(this Distance d) => (int)d / Coefficients.metres;

        // Time
        public static int ToMilliseconds(this Time t) => (int)t / Coefficients.milliseconds;

        public static int ToSeconds(this Time t) => (int)t / Coefficients.seconds;

        public static int ToMinutes(this Time t) => (int)t / Coefficients.minutes;

        public static int ToHours(this Time t) => (int)t / Coefficients.hours;

        public static int ToDays(this Time t) => (int)t / Coefficients.days;

        // Speed
        public static int ToMetresPerSecond(this Speed s) => (int)s / Coefficients.metresPerSecond;

        public static int ToKilometresPerHour(this Speed s) => (int)s / Coefficients.kilometresPerHour;

        // Acceleration
        public static int ToMetresPerSecondPerSecond(this Acceleration a) =>
            (int)a / Coefficients.metresPerSecondPerSecond;

        #endregion from_quant_to_int


        public static Weight Weight(this Time value) => new Weight(value);

        public static Weight Weight(this Speed value) => new Weight(value);
    }
}
