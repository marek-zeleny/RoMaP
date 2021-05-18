using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Components;

namespace RoadTrafficSimulator.GUI
{
    interface IGRoad
    {
        Coords From { get; }
        Coords To { get; }
        bool IsTwoWay { get; }
        Highlight Highlight { set; }
        IEnumerable<Road> GetRoads();
        IEnumerable<Coords> GetRoute();
        void Draw(Graphics graphics, Point from, Point to, int width);
    }

    interface IMutableRoad : IGRoad
    {
        public enum Direction : byte { Forward, Backward }

        IList<Coords> Route { get; }
        void SetRoad(Road road, Direction direction);
    }
}
