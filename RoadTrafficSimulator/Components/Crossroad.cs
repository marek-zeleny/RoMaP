using System;
using System.Collections.Generic;
using System.Text;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Crossroad : Node<int, int>
    {
        public Crossroad(int id)
            : base(id)
        {

        }
    }
}
