using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface ICrossroad
    {
        Highlight Highlight { set; }
        Coords CrossroadId { get; }
        void Draw(Graphics graphics, Point point, int size);
    }
}
