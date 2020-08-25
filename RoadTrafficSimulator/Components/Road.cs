using System;
using System.Collections.Generic;
using System.Text;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Road : Edge<int, int>
    {
        public Road(int id, Crossroad from, Crossroad to, Kilometers length, KilometersPerHour maxSpeed)
            : base(id, from, to, length / maxSpeed)
        {

        }
    }
}
