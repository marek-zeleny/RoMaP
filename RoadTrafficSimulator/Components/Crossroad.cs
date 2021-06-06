﻿using System;
using System.Collections.Generic;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Crossroad : Node<Coords, int>
    {
        private byte carSpawnRate = 10;

        public TrafficLight TrafficLight { get; }
        public PriorityCrossing PriorityCrossing { get; }
        public byte CarSpawnRate
        {
            get => carSpawnRate;
            set
            {
                if (value > 100)
                    carSpawnRate = 100;
                else if (value < 1)
                    carSpawnRate = 1;
                else
                    carSpawnRate = value;
            }
        }

        public Crossroad(Coords id)
            : base(id)
        {
            TrafficLight = new TrafficLight();
            PriorityCrossing = new PriorityCrossing();
        }

        public bool Initialise()
        {
            var trafficLightVerifier = new Dictionary<Direction, bool>(InDegree * OutDegree);
            foreach (Road inRoad in GetInEdges())
                foreach (Road outRoad in GetOutEdges())
                    trafficLightVerifier.Add(new Direction(inRoad.Id, outRoad.Id), false);
            return TrafficLight.Initialize(trafficLightVerifier);
        }

        public void Tick(Time time)
        {
            TrafficLight.Tick(time);
        }

        public bool CanCross(int fromRoadId, int toRoadId)
        {
            if (TrafficLight.Settings.Count > 1)
                return TrafficLight.CanCross(fromRoadId, toRoadId);
            else
                return PriorityCrossing.CanCross(fromRoadId, toRoadId);
        }

        public override void AddInEdge(IEdge<Coords, int> edge)
        {
            base.AddInEdge(edge);
            PriorityCrossing.AddInRoad(this, edge.Id);
        }

        public override void AddOutEdge(IEdge<Coords, int> edge)
        {
            base.AddOutEdge(edge);
            PriorityCrossing.AddOutRoad(this, edge.Id);
        }

        public override bool RemoveInEdge(int id)
        {
            bool result = base.RemoveInEdge(id);
            if (result)
            {
                TrafficLight.RemoveEdge(id);
                PriorityCrossing.RemoveInRoad(id);
            }
            return result;
        }

        public override bool RemoveOutEdge(int id)
        {
            bool result = base.RemoveOutEdge(id);
            if (result)
            {
                TrafficLight.RemoveEdge(id);
                PriorityCrossing.RemoveOutRoad(id);
            }
            return result;
        }
    }
}
