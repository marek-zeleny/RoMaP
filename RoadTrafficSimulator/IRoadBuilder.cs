using System;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.GUI;

namespace RoadTrafficSimulator
{
    interface IRoadBuilder
    {
        bool CanContinue { get; }
        bool AddSegment(Coords nextCoords);
        bool FinishRoad();
        bool FinishRoad(Speed maxSpeed);
        bool FinishRoad(out IGRoad builtRoad);
        bool FinishRoad(Speed maxSpeed, out IGRoad builtRoad);
        void DestroyRoad();
    }
}
