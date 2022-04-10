using System;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.GUI;

namespace RoadTrafficSimulator
{
    interface IRoadBuilder
    {
        bool CanContinue { get; }
        bool AddSegment(Coords nextCoords);
        bool FinishRoad(bool updatePriorityCrossing = true);
        bool FinishRoad(Speed maxSpeed, bool updatePriorityCrossing = true);
        bool FinishRoad(out IGRoad builtRoad, bool updatePriorityCrossing = true);
        bool FinishRoad(Speed maxSpeed, out IGRoad builtRoad, bool updatePriorityCrossing = true);
        void DestroyRoad();
    }
}
