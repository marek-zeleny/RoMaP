using System;
using System.Collections.Generic;
using System.Diagnostics;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Map : IReadOnlyGraph<Coords, int>
    {
        private int nextRoadId;

        private Graph<Coords, int> graph = new Graph<Coords, int>();

        public int CrossroadCount => graph.NodeCount;
        public int RoadCount => graph.EdgeCount;
        int IReadOnlyGraph<Coords, int>.NodeCount => graph.NodeCount;
        int IReadOnlyGraph<Coords, int>.EdgeCount => graph.EdgeCount;

        public Road AddRoad(Road originalRoad)
        {
            Debug.Assert(!originalRoad.IsConnected);
            // Cannot rely on the original crossroads still being valid
            Crossroad from = GetOrCreateCrossroad(originalRoad.FromNode.Id);
            Crossroad to = GetOrCreateCrossroad(originalRoad.ToNode.Id);
            int id = originalRoad.Id;
            if (graph.GetEdge(originalRoad.Id) != default)
            {
                // cannot reuse original ID
                id = nextRoadId++;
            }
            Road road = new Road(id, from, to, originalRoad);
            graph.AddEdge(road);
            road.IsConnected = true;
            return road;
        }

        public Road AddRoad(Coords fromId, Coords toId, Distance length, Speed maxSpeed)
        {
            Crossroad from = GetOrCreateCrossroad(fromId);
            Crossroad to = GetOrCreateCrossroad(toId);
            Road road = new Road(nextRoadId++, from, to, length, maxSpeed);
            graph.AddEdge(road);
            road.IsConnected = true;
            return road;
        }

        public Road RemoveRoad(int id)
        {
            Road output = (Road)graph.RemoveEdge(id);
            output.IsConnected = false;
            // Remove crossroads if they were connected only to this road
            graph.RemoveNode(output.FromNode.Id);
            graph.RemoveNode(output.ToNode.Id);
            return output;
        }

        public Crossroad RemoveCrossroad(Coords id)
        {
            Crossroad output = (Crossroad)graph.RemoveNodeForced(id, out var removedInRoads, out var removedOutRoads);
            // Remove crossroads that became stand-alone by removing this crossroad (more precisely the roads connected to this crossroad)
            foreach (var road in removedInRoads)
                graph.RemoveNode(road.FromNode.Id);
            foreach (var road in removedOutRoads)
                graph.RemoveNode(road.ToNode.Id);
            return output;
        }

        public IReadOnlyNode<Coords, int> GetNode(Coords id) => graph.GetNode(id);

        public IReadOnlyEdge<Coords, int> GetEdge(int id) => graph.GetEdge(id);

        public IEnumerable<IReadOnlyNode<Coords, int>> GetNodes() => graph.GetNodes();

        public IEnumerable<IReadOnlyEdge<Coords, int>> GetEdges() => graph.GetEdges();

        private Crossroad GetOrCreateCrossroad(Coords id)
        {
            Crossroad crossroad = (Crossroad)graph.GetNode(id);
            if (crossroad == null)
            {
                crossroad = new Crossroad(id);
                graph.AddNode(crossroad);
            }
            return crossroad;
        }
    }
}
