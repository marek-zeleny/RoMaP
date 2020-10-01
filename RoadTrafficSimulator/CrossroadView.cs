using System;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class CrossroadView
    {
        private readonly Components.Crossroad crossroad;

        public Coords Coords { get => crossroad.Id; }
        public int InIndex { get => crossroad.InDegree; }
        public int OutIndex { get => crossroad.OutDegree; }
        public TrafficLight TrafficLight { get => crossroad.TrafficLight; }
        public ICrossroad GuiCrossroad { get; }

        public CrossroadView(Components.Crossroad crossroad, ICrossroad guiCrossroad)
        {
            this.crossroad = crossroad;
            GuiCrossroad = guiCrossroad;
        }
    }
}
