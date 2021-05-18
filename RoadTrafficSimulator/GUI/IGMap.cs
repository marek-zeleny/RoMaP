using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface IGMap
    {
        bool AddCrossroad(IGCrossroad crossroad, Coords coords);
        bool AddRoad(IGRoad road, Vector vector);
        bool RemoveCrossroad(Coords coords);
        bool RemoveRoad(Vector vector);
        IGCrossroad GetCrossroad(Coords coords);
        IGRoad GetRoad(Vector vector, bool ignoreDirection = false);
        IEnumerable<IGCrossroad> GetCrossroads();
        IEnumerable<IGRoad> GetRoads();
        void Draw(Graphics graphics, Point origin, float zoom, int width, int height);
    }

    enum Highlight : sbyte
    {
        Low = -1,
        Normal = 0,
        High = 1
    }
}
