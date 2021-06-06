using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Components;

namespace RoadTrafficSimulator.GUI
{
    interface IGRoad
    {
        public enum Direction : byte { Forward, Backward }

        Coords From { get; }
        Coords To { get; }
        bool IsTwoWay { get; }
        Road GetRoad(Direction direction = Direction.Forward);
        IEnumerable<Road> GetRoads();
        IEnumerable<Coords> GetRoute(Direction direction = Direction.Forward);
        IGRoad GetReversedGRoad();
        void Highlight(Highlight highlight);
        void Highlight(Highlight highlight, Direction direction);
        void Draw(Graphics graphics, Point from, Point to, int width, bool simulationMode);
    }

    interface IMutableGRoad : IGRoad
    {
        ICollection<Coords> Route { get; }
        void SetRoad(Road road, Direction direction);
    }
}
