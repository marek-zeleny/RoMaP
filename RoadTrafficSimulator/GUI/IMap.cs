using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface IMap
    {
        bool AddCrossroad(ICrossroad crossroad, Coords coords);
        bool AddRoad(IRoad road, Vector vector);
        bool RemoveCrossroad(Coords coords);
        bool RemoveRoad(Vector vector);
        ICrossroad GetCrossroad(Coords coords);
        IRoad GetRoad(Vector vector, bool ignoreDirection = false);
        void Draw(Graphics graphics, Point origin, decimal zoom, int width, int height);
    }

    enum Highlight { Low = -1, Normal = 0, High = 1 }
}
