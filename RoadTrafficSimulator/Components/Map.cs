using System;
using System.Collections.Generic;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    struct Coords : IEquatable<Coords>
    {
        public readonly int x;
        public readonly int y;

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Coords other) => x == other.x && y == other.y;
    }

    class Map : IReadOnlyGraph<Coords, int>
    {
        private int nextRoadId;

        private DirectedGraph<Coords, int> graph = new DirectedGraph<Coords, int>();

        public int CrossroadCount => graph.NodeCount;
        public int RoadCount => graph.EdgeCount;
        int IReadOnlyGraph<Coords, int>.NodeCount => graph.NodeCount;
        int IReadOnlyGraph<Coords, int>.EdgeCount => graph.EdgeCount;

        public Road AddRoad(Coords fromId, Coords toId, Meters length, MetersPerSecond maxSpeed)
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

        public Road RemoveRoad(int id) => (Road)graph.RemoveEdge(id);

        public Crossroad RemoveCrossroad(Coords id) => (Crossroad)graph.RemoveNodeForced(id);

        public IReadOnlyNode<Coords, int> GetNode(Coords id) => graph.GetNode(id);

        public IReadOnlyEdge<Coords, int> GetEdge(int id) => graph.GetEdge(id);

        public IEnumerable<IReadOnlyNode<Coords, int>> GetNodes() => graph.GetNodes();

        public IEnumerable<IReadOnlyEdge<Coords, int>> GetEdges() => graph.GetEdges();
    }
}
