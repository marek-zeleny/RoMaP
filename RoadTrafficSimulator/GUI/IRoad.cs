using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface IRoad
    {
        bool IsTwoWay { get; }
        Highlight Highlight { set; }
        IEnumerable<int> GetRoadIds();
        IEnumerable<Coords> GetRoute();
        void Draw(Graphics graphics, Point from, Point to, int width);
    }

    interface IMutableRoad : IRoad
    {
        IList<Coords> Route { get; }
        void SetRoadId(int id, Direction direction = Direction.Forward);
        public enum Direction { Forward, Backward }
    }
}
