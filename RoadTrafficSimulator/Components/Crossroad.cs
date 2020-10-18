using System;
using System.Collections.Generic;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Crossroad : Node<Coords, int>
    {
        public TrafficLight TrafficLight { get; }

        public Crossroad(Coords id)
            : base(id)
        {
            TrafficLight = new TrafficLight();
        }

        public bool Initialize()
        {
            Dictionary<TrafficLight.Direction, bool> trafficLightVerifier = new Dictionary<TrafficLight.Direction, bool>(InDegree * OutDegree);
            foreach (Road inRoad in GetInEdges())
                foreach (Road outRoad in GetOutEdges())
                    trafficLightVerifier.Add(new TrafficLight.Direction(inRoad.Id, outRoad.Id), false);
            return TrafficLight.Initialize(trafficLightVerifier);
        }

        public void Tick(Seconds time)
        {
            TrafficLight.Tick(time);
        }

        public override bool RemoveInEdge(int id)
        {
            bool result = base.RemoveInEdge(id);
            if (result)
                foreach (var setting in TrafficLight.Settings)
                    setting.RemoveDirectionsWithFromId(id);
            return result;
        }

        public override bool RemoveOutEdge(int id)
        {
            bool result = base.RemoveOutEdge(id);
            if (result)
                foreach (var setting in TrafficLight.Settings)
                    setting.RemoveDirectionsWithToId(id);
            return result;
        }
    }
}
