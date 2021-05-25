using System;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class RoadView
    {
        private readonly Road road;

        public int Id { get => road.Id; }
        public Coords From { get => road.FromNode.Id; }
        public Coords To { get => road.ToNode.Id; }
        public Speed MaxSpeed { get => road.MaxSpeed; set => road.MaxSpeed = value; }
        public bool TwoWayRoad { get => GuiRoad.IsTwoWay; }
        public Road.IRoadStatistics Statistics { get => road.Statistics; }
        public IGRoad GuiRoad { get; }

        public RoadView(Road road, IGRoad guiRoad)
        {
            this.road = road;
            GuiRoad = guiRoad;
        }
    }
}
