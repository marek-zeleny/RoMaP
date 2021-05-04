using System;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Statistics
{
    interface IClock
    {
        Time Time { get; }
    }
}
