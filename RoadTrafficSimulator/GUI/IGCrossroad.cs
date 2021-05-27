﻿using System;
using System.Drawing;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.GUI
{
    interface IGCrossroad
    {
        Highlight Highlight { set; }
        Coords CrossroadId { get; }
        (Coords, Coords)? MainRoadDirections { get; set; }
        void Draw(Graphics graphics, Point point, int size);
    }
}
