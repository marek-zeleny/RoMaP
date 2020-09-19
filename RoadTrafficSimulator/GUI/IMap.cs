using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface IMap
    {
        bool AddCrossroad(ICrossroad crossroad, Coords coords);
        bool AddRoadSegment(IRoadSegment roadSegment, Vector vector);
        ICrossroad GetCrossroad(Coords coords);
        IRoadSegment GetRoadSegment(Vector vector);
        void Draw(Graphics graphics, Coords origin, decimal zoom, int width, int height);
    }
    
    struct Vector
    {
        public readonly Coords from;
        public readonly Coords to;
    }

    enum Highlight { Low, Normal, High }
}
