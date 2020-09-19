using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoadTrafficSimulator.GUI
{
    interface IRoadSegment
    {
        Highlight Highlight { set; }
        IEnumerable<int> GetRoadIds();
        void Draw(Graphics graphics, Point from, Point to, int width);
    }
}
