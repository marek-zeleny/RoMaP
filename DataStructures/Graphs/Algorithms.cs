using System;
using System.Collections.Generic;
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
        /// Finds a shortest path in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> between two
        /// <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/>s.
        /// </summary>
        /// <remarks>
        /// Algorithm for finding the path is chosen based on <paramref name="graphType"/>.
        /// <see cref="GraphType.Acyclic"/> uses DAG (Directed Acyclic Graph) algorithm with time complexity
        /// O(<c>n</c> + <c>m</c>),
        /// <see cref="GraphType.NonnegativeWeights"/> uses Dijkstra's algorithm with time complexity
        /// O((<c>n</c> + <c>m</c>) * log(<c>n</c>)),
        /// <see cref="GraphType.General"/> uses Bellman-Ford algorithm with time complexity O(<c>n</c> * <c>m</c>)
        /// for a graph with <c>n</c> nodes and <c>m</c> edges.
        /// If the graph doesn't fulfil requirements for the given <paramref name="graphType"/>, the behaviour is
        /// undefined.
        /// </remarks>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to search.</param>
        /// <param name="graphType">
        /// <see cref="GraphType"/> of the graph. This parameter determines what search algorithm is used.
        /// See remarks for more information.
        /// </param>
        /// <param name="startNode">Starting <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> of the path.</param>
        /// <param name="endNode">Ending <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> of the path.</param>
        /// <param name="pathWeight">
        /// Outputs total weight of the shortest path that was found. If the search isn't successful, the value is
        /// undefined.
        /// </param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> of type <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with edges on the
        /// shortest path from <paramref name="startNode"/> to <paramref name="endNode"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startNode"/> or <paramref name="endNode"/> is not present in the <paramref name="graph"/>.
        /// </exception>
        /// <exception cref="InconsistentGraphException{TNodeId, TEdgeId}">
        /// <paramref name="graph"/> is not consistent.
        /// </exception>
        public static IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> FindShortestPath<TNodeId, TEdgeId>(
            this IReadOnlyGraph<TNodeId, TEdgeId> graph,
            GraphType graphType,
            IReadOnlyNode<TNodeId, TEdgeId> startNode,
            IReadOnlyNode<TNodeId, TEdgeId> endNode,
            out Weight pathWeight
            )
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            switch(graphType)
            {
                case GraphType.NonnegativeWeights:
                    return graph.DijkstrasAlgorithm(startNode, endNode, out pathWeight);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion interface

        #region implementation

        /// <summary>
        /// Finds a shortest path in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> with nonnegative weights between
        /// two <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/>s using Dijkstra's algorithm.
        /// </summary>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to search.</param>
        /// <param name="startNode">Starting <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> of the path.</param>
        /// <param name="endNode">Ending <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> of the path.</param>
        /// <param name="pathWeight">
        /// Outputs total weight of the shortest path that was found. If the search isn't successful, the value is
        /// undefined.
        /// </param>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> of type <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with edges on the
        /// shortest path from <paramref name="startNode"/> to <paramref name="endNode"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startNode"/> or <paramref name="endNode"/> is not present in the <paramref name="graph"/>.
        /// </exception>
        /// <exception cref="InconsistentGraphException{TNodeId, TEdgeId}">
        /// <paramref name="graph"/> is not consistent.
        /// </exception>
        private static IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> DijkstrasAlgorithm<TNodeId, TEdgeId>(
            this IReadOnlyGraph<TNodeId, TEdgeId> graph,
            IReadOnlyNode<TNodeId, TEdgeId> startNode,
            IReadOnlyNode<TNodeId, TEdgeId> endNode,
            out Weight pathWeight
            )
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            var unsolvedNodes = new BinaryHeap<Path<TNodeId, TEdgeId>, IReadOnlyNode<TNodeId, TEdgeId>>(
                graph.GetNodes(),
                () => new Path<TNodeId, TEdgeId>());
            try
            {
                unsolvedNodes.DecreaseKey(startNode, path => path.TotalWeight = 0.Weight());
            }
            catch (ValueNotFoundException e)
            {
                throw new ArgumentException($"{nameof(startNode)} is not present in the graph.", nameof(startNode), e);
            }
            var solvedNodes = new HashSet<IReadOnlyNode<TNodeId, TEdgeId>>();

            while (!unsolvedNodes.IsEmpty
                && unsolvedNodes.PeekMin().Value != endNode
                && unsolvedNodes.PeekMin().Key.TotalWeight < double.PositiveInfinity
                )
            {
                var (path, node) = unsolvedNodes.ExtractMin();
                solvedNodes.Add(node);
                foreach (var edge in node.GetOutEdges())
                {
                    if (solvedNodes.Contains(edge.ToNode))
                        continue;
                    Weight currWeight = unsolvedNodes[edge.ToNode].TotalWeight;
                    Weight newWeight = path.TotalWeight + edge.Weight;
                    if (newWeight < currWeight)
                    {
                        try
                        {
                            unsolvedNodes.DecreaseKey(
                                edge.ToNode,
                                currPath =>
                                {
                                    currPath.Edges = path.Edges.AddFront(edge);
                                    currPath.TotalWeight = newWeight;
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

            if (unsolvedNodes.IsEmpty)
                throw new ArgumentException($"{nameof(endNode)} is not present in the graph.", nameof(endNode));
            var (foundPath, _) = unsolvedNodes.PeekMin();
            pathWeight = foundPath.TotalWeight;
            return foundPath.Edges.Reverse();
        }

        #region helper_classes

        /// <summary>
        /// Path of edges in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>.
        /// </summary>
        private class Path<TNodeId, TEdgeId>
            : IComparable<Path<TNodeId, TEdgeId>>
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            /// <summary>
            /// Gets or sets edges of the path.
            /// </summary>
            public SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>> Edges { get; set; }
            /// <summary>
            /// Gets or sets total weight of the path.
            /// </summary>
            public Weight TotalWeight { get; set; }

            /// <summary>
            /// Creates an empty path with infinite total weight.
            /// </summary>
            public Path()
            {
                Edges = new SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>>();
                TotalWeight = double.PositiveInfinity.Weight();
            }

            /// <summary>
            /// Creates a new path with given <paramref name="edges"/> and <paramref name="totalWeight"/>.
            /// </summary>
            /// <param name="edges">Edges on the path.</param>
            /// <param name="totalWeight">Total weight of the path.</param>
            public Path(SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>> edges, Weight totalWeight)
            {
                Edges = edges;
                TotalWeight = totalWeight;
            }

            public int CompareTo(Path<TNodeId, TEdgeId> other) => TotalWeight.CompareTo(other.TotalWeight);

            public override string ToString() => $"Path: TW{TotalWeight}";
        }

        #endregion helper_classes

        #endregion implementation
    }
}
