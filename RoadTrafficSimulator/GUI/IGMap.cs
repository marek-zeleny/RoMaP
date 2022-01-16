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
        IGRoad GetRoad(Vector vector);
        IEnumerable<IGCrossroad> GetCrossroads();
        IEnumerable<IGRoad> GetRoads();
        void Draw(Graphics graphics, Point origin, float zoom, int width, int height, bool simulationMode);
    }

    [Flags]
    enum Highlight : byte
    {
        None = 0,
        Transparent = 1,
        Large = 2,
    }
}
