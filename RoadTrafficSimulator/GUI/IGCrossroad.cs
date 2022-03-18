using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface IGCrossroad
    {
        Coords CrossroadId { get; }
        (Coords, Coords)? MainRoadDirections { get; set; }
        void ResetHighlight(Highlight highlight);
        void SetHighlight(Highlight highlight);
        void UnsetHighlight(Highlight highlight);
        void Draw(Graphics graphics, Point origin, float zoom, Func<Point, bool> isVisible);
    }
}
