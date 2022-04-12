using System;
using System.Collections.Generic;
using System.Diagnostics;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    /// <summary>
    /// Represents a map of roads and crossroads.
    /// </summary>
    /// <remarks>
    /// The map is only an abstract representation (a graph) - it holds no spacial information.
    /// </remarks>
    class Map : IReadOnlyGraph<Coords, int>
    {
        private int nextRoadId;

        private Graph<Coords, int> graph = new();

        /// <summary>
        /// Number of crossroads in the map
        /// </summary>
        public int CrossroadCount => graph.NodeCount;
        /// <summary>
        /// Number of roads in the map
        /// </summary>
        public int RoadCount => graph.EdgeCount;
        int IReadOnlyGraph<Coords, int>.NodeCount => graph.NodeCount;
        int IReadOnlyGraph<Coords, int>.EdgeCount => graph.EdgeCount;

        /// <summary>
        /// Adds a new road to the map with the same properties as a given existing road that is no longer in the map.
        /// </summary>
        /// <remarks>
        /// This method can be used to restore a previously removed road.
        /// Note that the original road cannot be inserted into the map because the crossroads it connects are
        /// immutable and may no longer exist in the map, so the road has to be cloned instead.
        /// </remarks>
        /// <param name="originalRoad">Road no longer in the map serving as a template for the added road</param>
        /// <returns>The new road added to the map</returns>
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
            Road road = new(id, from, to, originalRoad);
            graph.AddEdge(road);
            road.IsConnected = true;
            return road;
        }

        /// <summary>
        /// Creates a new road with the given properties and adds it to the map.
        /// </summary>
        /// <remarks>
        /// If the starting or ending crossroad doesn't yet exist in the map, it will be added as well.
        /// </remarks>
        /// <param name="fromId">ID of a crossroad where the road begins</param>
        /// <param name="toId">ID of a crossroad where the road ends</param>
        /// <param name="length">Length of the road</param>
        /// <param name="maxSpeed">Maximum allowed speed on the road</param>
        /// <returns>The new road added to the map</returns>
        public Road AddRoad(Coords fromId, Coords toId, Distance length, Speed maxSpeed)
        {
            Crossroad from = GetOrCreateCrossroad(fromId);
            Crossroad to = GetOrCreateCrossroad(toId);
            Road road = new(nextRoadId++, from, to, length, maxSpeed);
            graph.AddEdge(road);
            road.IsConnected = true;
            return road;
        }

        /// <summary>
        /// Removes the road with the given ID from the map.
        /// </summary>
        /// <remarks>
        /// If a crossroad becomes isolated as a result of this operation, it's removed as well.
        /// </remarks>
        /// <returns>The removed road</returns>
        public Road RemoveRoad(int id)
        {
            Road output = (Road)graph.RemoveEdge(id);
            output.IsConnected = false;
            // Remove crossroads if they were connected only to this road
            graph.RemoveNode(output.FromNode.Id);
            graph.RemoveNode(output.ToNode.Id);
            return output;
        }

        /// <summary>
        /// Removes the crossroad with the given ID from the map, as well as any roads connected to that crossroad.
        /// </summary>
        /// <remarks>
        /// If another crossroad becomes isolated as a result of this operation, it's removed as well.
        /// </remarks>
        /// <returns>The removed crossroad</returns>
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

        /// <summary>
        /// Gets the crossroad with the given ID. If such crossroad doesn't exist, it's created and added to the map.
        /// </summary>
        /// <returns>Existing or new crossroad with the given ID</returns>
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
