﻿using System;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    interface IRoadBuilder
    {
        bool CanContinue { get; }
        bool AddSegment(Coords nextCoords);
        bool FinishRoad(MetersPerSecond maxSpeed);
        void DestroyRoad();
    }
}
