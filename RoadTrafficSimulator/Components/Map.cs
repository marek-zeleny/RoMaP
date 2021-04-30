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

        public Road AddRoad(Road road)
        {
            Debug.Assert(road.Id < nextRoadId);
            Debug.Assert(graph.GetEdge(road.Id) == default);
            graph.AddNode((Crossroad)road.FromNode);
            graph.AddNode((Crossroad)road.ToNode);
            bool result = graph.AddEdge(road);
            Debug.Assert(result);
            return road;
        }

        public Road AddRoad(Coords fromId, Coords toId, Millimetres length, MillimetresPerSecond maxSpeed)
        {
            Crossroad from = (Crossroad)graph.GetNode(fromId);
            Crossroad to = (Crossroad)graph.GetNode(toId);
            if (from == null)
            {
                from = new Crossroad(fromId);
                graph.AddNode(from);
            }
            if (to == null)
            {
                to = new Crossroad(toId);
                graph.AddNode(to);
            }
            Road road = new Road(nextRoadId++, from, to, length, maxSpeed);
            graph.AddEdge(road);
            return road;
        }

        public Road RemoveRoad(int id)
        {
            Road output = (Road)graph.RemoveEdge(id);
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
    }
}
