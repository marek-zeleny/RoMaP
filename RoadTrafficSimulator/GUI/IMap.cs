using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface IMap
    {
        bool AddCrossroad(ICrossroad crossroad, Coords coords);
        bool AddRoadSegment(IRoadSegment roadSegment, Vector vector);
        bool RemoveCrossroad(Coords coords);
        bool RemoveRoadSegment(Vector vector);
        ICrossroad GetCrossroad(Coords coords);
        IRoadSegment GetRoadSegment(Vector vector, bool ignoreDirection = false);
        void Draw(Graphics graphics, Point origin, decimal zoom, int width, int height);
    }

    enum Highlight { Low, Normal = 0, High }
}
