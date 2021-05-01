﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using DataStructures.Miscellaneous;

namespace DataStructures.Graphs
{
    /// <summary>
    /// Class providing algorithms on data structures.
    /// </summary>
    public static class Algorithms
    {
        #region interface

        /// <summary>
        /// Classification types of graphs.
        /// </summary>
        public enum GraphType
        {
            /// <summary>
            /// Graph without (directed) cycles.
            /// </summary>
            Acyclic,
            /// <summary>
            /// Graph with all edge weights greater than or equal to zero.
            /// </summary>
            NonnegativeWeights,
            /// <summary>
            /// Graph without any restrictions - allowed cycles, negative weights.
            /// </summary>
            General
        };

        /// <summary>
        /// Path of edges in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>.
        /// </summary>
        /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
        public readonly struct Path<TNodeId, TEdgeId>
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            /// <summary>
            /// Gets edges on the path.
            /// </summary>
            public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> Edges { get; }
            /// <summary>
            /// Gets total weight of the path.
            /// </summary>
            public Weight Weight { get; }

            /// <summary>
            /// Creates a new path with given <paramref name="edges"/> and <paramref name="weight"/>.
            /// </summary>
            /// <param name="edges">Edges on the path.</param>
            /// <param name="weight">Total weight of the path.</param>
            public Path(IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> edges, Weight weight)
            {
                Edges = edges;
                Weight = weight;
            }

            public override string ToString() => $"Path: W{Weight}";
        }

        /// <summary>
        /// Finds a shortest path in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> between two
        /// <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/>s.
        /// </summary>
        /// <remarks>
        /// Algorithm for finding the path is chosen based on <paramref name="graphType"/>.
        /// <list type="bullet">
        /// <item>
        /// <see cref="GraphType.Acyclic"/> uses DAG (Directed Acyclic Graph) algorithm with time complexity
        /// O(<c>n</c> + <c>m</c>),
        /// </item>
        /// <item>
        /// <see cref="GraphType.NonnegativeWeights"/> uses Dijkstra's algorithm with time complexity
        /// O((<c>n</c> + <c>m</c>) * log(<c>n</c>)),
        /// </item>
        /// <item>
        /// <see cref="GraphType.General"/> uses Bellman-Ford algorithm with time complexity O(<c>n</c> * <c>m</c>)
        /// for a graph with <c>n</c> nodes and <c>m</c> edges.
        /// </item>
        /// </list>
        /// If <paramref name="graph"/> doesn't fulfil requirements for the given <paramref name="graphType"/>, the behaviour is
        /// undefined.
        /// </remarks>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to search.</param>
        /// <param name="graphType">
        /// <see cref="GraphType"/> of the graph. This parameter determines what search algorithm is used.
        /// See remarks for more information.
        /// </param>
        /// <param name="startNode">ID of the first <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> on the path.</param>
        /// <param name="endNode">ID of the last <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> on the path.</param>
        /// <returns>
        /// <see cref="Path{TNodeId, TEdgeId}"/> containing a shortest path from <paramref name="startNode"/> to
        /// <paramref name="endNode"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startNode"/> or <paramref name="endNode"/> is not present in the <paramref name="graph"/>.
        /// </exception>
        /// <exception cref="InconsistentGraphException{TNodeId, TEdgeId}">
        /// <paramref name="graph"/> is not consistent.
        /// </exception>
        /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
        public static Path<TNodeId, TEdgeId> FindShortestPath<TNodeId, TEdgeId>(
            this IReadOnlyGraph<TNodeId, TEdgeId> graph,
            GraphType graphType,
            TNodeId startNode,
            TNodeId endNode
            )
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            var start = graph.GetNode(startNode);
            if (start == default)
                throw new ArgumentException($"Node with ID {startNode} is not present in the graph.",
                    nameof(startNode));
            var end = graph.GetNode(endNode);
            if (end == default)
                throw new ArgumentException($"Node with ID {endNode} is not present in the graph.",
                    nameof(endNode));

            switch (graphType)
            {
                case GraphType.NonnegativeWeights:
                    return graph.DijkstrasAlgorithm(start, end)[end.Id];
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Finds shortest paths in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> from a
        /// <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> to all other nodes.
        /// </summary>
        /// <remarks>
        /// Algorithm for finding the path is chosen based on <paramref name="graphType"/>.
        /// <list type="bullet">
        /// <item>
        /// <see cref="GraphType.Acyclic"/> uses DAG (Directed Acyclic Graph) algorithm with time complexity
        /// O(<c>n</c> + <c>m</c>),
        /// </item>
        /// <item>
        /// <see cref="GraphType.NonnegativeWeights"/> uses Dijkstra's algorithm with time complexity
        /// O((<c>n</c> + <c>m</c>) * log(<c>n</c>)),
        /// </item>
        /// <item>
        /// <see cref="GraphType.General"/> uses Bellman-Ford algorithm with time complexity O(<c>n</c> * <c>m</c>)
        /// for a graph with <c>n</c> nodes and <c>m</c> edges.
        /// </item>
        /// </list>
        /// If <paramref name="graph"/> doesn't fulfil requirements for the given <paramref name="graphType"/>, the behaviour is
        /// undefined.
        /// </remarks>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to search.</param>
        /// <param name="graphType">
        /// <see cref="GraphType"/> of the graph. This parameter determines what search algorithm is used.
        /// See remarks for more information.
        /// </param>
        /// <param name="startNode">ID of the first <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> on the path.</param>
        /// <returns>
        /// <see cref="IDictionary{TKey, TValue}"/> with <typeparamref name="TNodeId"/> as key and
        /// <see cref="Path{TNodeId, TEdgeId}"/> as value, containing shortest paths from <paramref name="startNode"/>
        /// to all other nodes in the <paramref name="graph"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startNode"/> is not present in the <paramref name="graph"/>.
        /// </exception>
        /// <exception cref="InconsistentGraphException{TNodeId, TEdgeId}">
        /// <paramref name="graph"/> is not consistent.
        /// </exception>
        /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
        public static IDictionary<TNodeId, Path<TNodeId, TEdgeId>> FindShortestPaths<TNodeId, TEdgeId>(
            this IReadOnlyGraph<TNodeId, TEdgeId> graph,
            GraphType graphType,
            TNodeId startNode
            )
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            var start = graph.GetNode(startNode);
            if (start == default)
                throw new ArgumentException($"Node with ID {startNode} is not present in the graph.",
                    nameof(startNode));

            switch (graphType)
            {
                case GraphType.NonnegativeWeights:
                    return graph.DijkstrasAlgorithm(start);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion interface

        #region implementation

        /// <summary>
        /// Finds shortest paths in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> with non-negative weights from a
        /// <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> to another node or to all other nodes in the
        /// <paramref name="graph"/> using Dijkstra's algorithm.
        /// </summary>
        /// <remarks>
        /// If <paramref name="endNode"/> is <c>null</c>, the returned dictionary will contain paths to all other nodes
        /// in the <paramref name="graph"/>. Otherwise, only <paramref name="endNode"/> guaranteed to be in the returned
        /// dictionary. This option is provided for better performance when only a path to a specific node is required.
        /// </remarks>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to search.</param>
        /// <param name="startNode">Starting <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> of the path.</param>
        /// <param name="endNode">
        /// Ending <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> of the path or <c>null</c> if paths to all other nodes
        /// are required.
        /// </param>
        /// <returns>
        /// <see cref="IDictionary{TKey, TValue}"/> with <typeparamref name="TNodeId"/> as key and
        /// <see cref="Path{TNodeId, TEdgeId}"/> as value, containing shortest paths from <paramref name="startNode"/>
        /// to other nodes in the <paramref name="graph"/>. See remarks for more detail.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startNode"/> or <paramref name="endNode"/> is not present in the <paramref name="graph"/>.
        /// </exception>
        /// <exception cref="InconsistentGraphException{TNodeId, TEdgeId}">
        /// <paramref name="graph"/> is not consistent.
        /// </exception>
        /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
        private static IDictionary<TNodeId, Path<TNodeId, TEdgeId>> DijkstrasAlgorithm<TNodeId, TEdgeId>(
            this IReadOnlyGraph<TNodeId, TEdgeId> graph,
            IReadOnlyNode<TNodeId, TEdgeId> startNode,
            IReadOnlyNode<TNodeId, TEdgeId> endNode = null
            )
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            // Initialisation
            var unsolvedNodes = new BinaryHeap<InternalPath<TNodeId, TEdgeId>, IReadOnlyNode<TNodeId, TEdgeId>>(
                graph.GetNodes(),
                () => new InternalPath<TNodeId, TEdgeId>());
            try
            {
                unsolvedNodes.DecreaseKey(startNode, path => path.Weight = 0.Weight());
            }
            catch (ValueNotFoundException e)
            {
                throw new ArgumentException($"{nameof(startNode)} is not present in the graph.", nameof(startNode), e);
            }
            var solvedNodes = new Dictionary<TNodeId, Path<TNodeId, TEdgeId>>();
            // Computation part of the algorithm
            while (!unsolvedNodes.IsEmpty
                && unsolvedNodes.PeekMin().Value != endNode
                && unsolvedNodes.PeekMin().Key.Weight < Weight.positiveInfinity
                )
            {
                var (path, node) = unsolvedNodes.ExtractMin();
                solvedNodes.Add(node.Id, new Path<TNodeId, TEdgeId>(path.Edges.Reverse(), path.Weight));
                foreach (var edge in node.GetOutEdges())
                {
                    if (solvedNodes.ContainsKey(edge.ToNode.Id))
                        continue;
                    Weight currWeight = unsolvedNodes[edge.ToNode].Weight;
                    Weight newWeight = path.Weight + edge.Weight;
                    if (newWeight < currWeight)
                    {
                        try
                        {
                            unsolvedNodes.DecreaseKey(
                                edge.ToNode,
                                currPath =>
                                {
                                    currPath.Edges = path.Edges.AddFront(edge);
                                    currPath.Weight = newWeight;
                                });
                        }
                        catch (ValueNotFoundException e)
                        {
                            throw new InconsistentGraphException<TNodeId, TEdgeId>(
                                "Ending node of an edge is not in the graph.", graph, edge, e
                                );
                        }
                    }
                }
            }

            if (endNode == null)
                // Adding unreachable nodes
                foreach (var (path, node) in unsolvedNodes)
                    // Edges are not reversed because they should be empty
                    solvedNodes.Add(node.Id, new Path<TNodeId, TEdgeId>(path.Edges, path.Weight));
            else
            {
                if (unsolvedNodes.IsEmpty)
                    throw new ArgumentException($"{nameof(endNode)} is not present in the graph.", nameof(endNode));
                var (path, node) = unsolvedNodes.PeekMin();
                Debug.Assert(node == endNode);
                solvedNodes.Add(node.Id, new Path<TNodeId, TEdgeId>(path.Edges.Reverse(), path.Weight));
            }
            return solvedNodes;
        }

        #region helper_classes

        /// <summary>
        /// Path of edges in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>.
        /// </summary>
        /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
        private class InternalPath<TNodeId, TEdgeId>
            : IComparable<InternalPath<TNodeId, TEdgeId>>
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            /// <summary>
            /// Gets or sets edges on the path.
            /// </summary>
            public SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>> Edges { get; set; }
            /// <summary>
            /// Gets or sets total weight of the path.
            /// </summary>
            public Weight Weight { get; set; }

            /// <summary>
            /// Creates an empty path with infinite total weight.
            /// </summary>
            public InternalPath()
            {
                Edges = new SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>>();
                Weight = Weight.positiveInfinity;
            }

            /// <summary>
            /// Creates a new path with given <paramref name="edges"/> and <paramref name="weight"/>.
            /// </summary>
            /// <param name="edges">Edges on the path.</param>
            /// <param name="weight">Total weight of the path.</param>
            public InternalPath(SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>> edges, Weight weight)
            {
                Edges = edges;
                Weight = weight;
            }

            public int CompareTo(InternalPath<TNodeId, TEdgeId> other) => Weight.CompareTo(other.Weight);

            public override string ToString() => $"Internal path: W{Weight}";
        }

        #endregion helper_classes

        #endregion implementation
    }
}
