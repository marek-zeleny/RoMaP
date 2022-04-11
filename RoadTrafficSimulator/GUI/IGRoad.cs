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
        void ResetHighlight(Highlight highlight);
        void ResetHighlight(Highlight highlight, Direction direction);
        void SetHighlight(Highlight highlight, Direction direction);
        void UnsetHighlight(Highlight highlight, Direction direction);
        void Draw(Graphics graphics, Point origin, float zoom, RoadSide sideOfDriving, bool simulationMode,
            Func<Point, bool> isVisible);
    }

    interface IMutableGRoad : IGRoad
    {
        ICollection<Coords> Route { get; }
        void SetRoad(Road road, Direction direction);
    }
}
