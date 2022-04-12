using System;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Statistics
{
    /// <summary>
    /// Provides a method for obtaining current time of some clock.
    /// </summary>
    interface IClock
    {
        /// <summary>
        /// Current time of the clock
        /// </summary>
        Time Time { get; }
    }
}
