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
        public byte CarSpawnRate { get => crossroad.CarSpawnRate; set => crossroad.CarSpawnRate = value; }
        public IGCrossroad GuiCrossroad { get; }

        public CrossroadView(Components.Crossroad crossroad, IGCrossroad guiCrossroad)
        {
            this.crossroad = crossroad;
            GuiCrossroad = guiCrossroad;
        }
    }
}
