using System;
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
        /// Determines whether a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> is strongly connected, i.e. there exists
        /// a path between any two nodes.
        /// </summary>
        /// <remarks>
        /// Time complexity of this algorithm is O(n + m).
        /// </remarks>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to check</param>
        /// <returns><c>true</c> if <paramref name="graph"/> is strongly connected, otherwise <c>false</c></returns>
        /// <exception cref="InconsistentGraphException{TNodeId, TEdgeId}">
        /// <paramref name="graph"/> is not consistent.
        /// </exception>
        /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
        public static bool IsStronglyConnected<TNodeId, TEdgeId>(this IReadOnlyGraph<TNodeId, TEdgeId> graph)
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            if (graph.NodeCount == 0)
                return true;
            // Initialisation
            IReadOnlyNode<TNodeId, TEdgeId> source = graph.GetNodes().First();
            HashSet<IReadOnlyNode<TNodeId, TEdgeId>> visitedNodes = new();
            Stack<IReadOnlyNode<TNodeId, TEdgeId>> processedNodes = new();
            processedNodes.Push(source);
            // Forward DFS
            while (processedNodes.TryPop(out var node))
            {
                visitedNodes.Add(node);
                foreach (var edge in node.GetOutEdges())
                    if (!visitedNodes.Contains(edge.ToNode))
                        processedNodes.Push(edge.ToNode);
            }
            // Check if we visited all nodes
            if (visitedNodes.Count < graph.NodeCount)
                return false;
            else if (visitedNodes.Count > graph.NodeCount)
                throw new InconsistentGraphException<TNodeId, TEdgeId>(
                    "DFS algorithm discovered more nodes than the graph contains.", graph);
            // Re-initialisation
            visitedNodes.Clear();
            processedNodes.Push(source);
            // Backward DFS
            while (processedNodes.TryPop(out var node))
            {
                visitedNodes.Add(node);
                foreach (var edge in node.GetInEdges())
                    if (!visitedNodes.Contains(edge.FromNode))
                        processedNodes.Push(edge.FromNode);
            }
            // Check if we visited all nodes again
            if (visitedNodes.Count > graph.NodeCount)
                throw new InconsistentGraphException<TNodeId, TEdgeId>(
                    "DFS algorithm discovered more nodes than the graph contains.", graph);
            return visitedNodes.Count == graph.NodeCount;
        }

        #region path_algorithms

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
            /// Segment of a <see cref="Path{TNodeId, TEdgeId}"/>.
            /// </summary>
            /// <remarks>
            /// Contains an edge and a total <see cref="Weight"/> of the path from the starting node up to this edge.
            /// </remarks>
            public readonly struct Segment
            {
                /// <summary>
                /// Gets the edge of forming the <see cref="Segment"/>.
                /// </summary>
                public IReadOnlyEdge<TNodeId, TEdgeId> Edge { get; }
                /// <summary>
                /// Gets total weight of a <see cref="Path{TNodeId, TEdgeId}"/> from the starting node up to the
                /// segment's <see cref="Edge"/>.
                /// </summary>
                public Weight TotalWeight { get; }

                /// <summary>
                /// Creates a new path segment of a given <paramref name="edge"/> and <paramref name="totalWeight"/>.
                /// </summary>
                /// <param name="edge">Edge forming the segment.</param>
                /// <param name="totalWeight">Total weight of the path up to the created segment.</param>
                public Segment(IReadOnlyEdge<TNodeId, TEdgeId> edge, Weight totalWeight)
                {
                    Edge = edge;
                    TotalWeight = totalWeight;
                }

                public override string ToString() => $"Path segment: TW{TotalWeight}";
            }

            /// <summary>
            /// Gets total weight of the path.
            /// </summary>
            public Weight TotalWeight { get; }
            /// <summary>
            /// Gets raw path segments forming the <see cref="Path{TNodeId, TEdgeId}"/>, ordered form end to start.
            /// </summary>
            /// <remarks>
            /// This is an O(1) operation.
            /// </remarks>
            public IEnumerable<Segment> ReversedPathSegments { get; }

            /// <summary>
            /// Creates a new path consisting of given <see cref="Segment"/>s.
            /// </summary>
            /// <param name="reversedPathSegments">Segments forming the path (ordered form end to start).</param>
            public Path(IEnumerable<Segment> reversedPathSegments)
            {
                TotalWeight = reversedPathSegments.First().TotalWeight;
                ReversedPathSegments = reversedPathSegments.SkipLast(1);
            }

            /// <summary>
            /// Gets edges forming the <see cref="Path{TNodeId, TEdgeId}"/> in the forward order (from start to end).
            /// </summary>
            /// <remarks>
            /// This is an O(<c>n</c>) operation for <c>n</c> edges, since the edges are stored in reversed order.
            /// </remarks>
            public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetEdges()
            {
                return ReversedPathSegments.Reverse().Select(segment => segment.Edge);
            }

            public override string ToString() => $"Path: W{TotalWeight}";
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
        /// If <paramref name="graph"/> doesn't fulfil requirements for the given <paramref name="graphType"/>, the
        /// behaviour is undefined.
        /// The algorithm can use a custom <paramref name="weightFunction"/> for computing weights. If no function is
        /// given, a default implementation is used (returns <see cref="IReadOnlyEdge{TNodeId, TEdgeId}.Weight"/>).
        /// </remarks>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to search.</param>
        /// <param name="graphType">
        /// <see cref="GraphType"/> of the graph. This parameter determines what search algorithm is used.
        /// See remarks for more information.
        /// </param>
        /// <param name="startNode">First <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> on the path.</param>
        /// <param name="endNode">Last <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> on the path.</param>
        /// <param name="weightFunction">
        /// Function for computing weight of an <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/>. If <c>null</c>, a default
        /// implementation will be used (see remarks).
        /// </param>
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
            IReadOnlyNode<TNodeId, TEdgeId> startNode,
            IReadOnlyNode<TNodeId, TEdgeId> endNode,
            Func<IReadOnlyEdge<TNodeId, TEdgeId>, Weight> weightFunction = null
            )
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            if (graph.GetNode(startNode.Id) == default)
                throw new ArgumentException($"Start node is not present in the graph.",
                    nameof(startNode));
            if (graph.GetNode(endNode.Id) == default)
                throw new ArgumentException($"End node is not present in the graph.",
                    nameof(endNode));

            return graphType switch
            {
                GraphType.NonnegativeWeights => graph.DijkstrasAlgorithm(startNode, endNode, weightFunction)[endNode],
                _ => throw new NotImplementedException(),
            };
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
        /// If <paramref name="graph"/> doesn't fulfil requirements for the given <paramref name="graphType"/>,
        /// the behaviour is undefined.
        /// The algorithm can use a custom <paramref name="weightFunction"/> for computing weights. If no function is
        /// given, a default implementation is used (returns <see cref="IReadOnlyEdge{TNodeId, TEdgeId}.Weight"/>).
        /// </remarks>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to search.</param>
        /// <param name="graphType">
        /// <see cref="GraphType"/> of the graph. This parameter determines what search algorithm is used.
        /// See remarks for more information.
        /// </param>
        /// <param name="startNode">Source <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> for searched paths.</param>
        /// <param name="weightFunction">
        /// Function for computing weight of an <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/>. If <c>null</c>, a default
        /// implementation will be used (see remarks).
        /// </param>
        /// <returns>
        /// <see cref="IDictionary{TKey, TValue}"/> with <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> as key and
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
        public static IDictionary<IReadOnlyNode<TNodeId, TEdgeId>, Path<TNodeId, TEdgeId>>
            FindShortestPaths<TNodeId, TEdgeId>(
            this IReadOnlyGraph<TNodeId, TEdgeId> graph,
            GraphType graphType,
            IReadOnlyNode<TNodeId, TEdgeId> startNode,
            Func<IReadOnlyEdge<TNodeId, TEdgeId>, Weight> weightFunction = null
            )
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            if (graph.GetNode(startNode.Id) == default)
                throw new ArgumentException($"Start node is not present in the graph.",
                    nameof(startNode));

            return graphType switch
            {
                GraphType.NonnegativeWeights => graph.DijkstrasAlgorithm(startNode, null, weightFunction),
                _ => throw new NotImplementedException(),
            };
        }

        #endregion path_algorithms

        #endregion interface

        #region implementation

        #region path_algorithms

        /// <summary>
        /// Finds shortest paths in a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> with non-negative weights from a
        /// <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> to another node or to all other nodes in the
        /// <paramref name="graph"/> using Dijkstra's algorithm.
        /// </summary>
        /// <remarks>
        /// If <paramref name="endNode"/> is <c>null</c>, the returned dictionary will contain paths to all other nodes
        /// in the <paramref name="graph"/>. Otherwise, only <paramref name="endNode"/> guaranteed to be in the returned
        /// dictionary. This option is provided for better performance when only a path to a specific node is required.
        /// The algorithm can use a custom <paramref name="weightFunction"/> for computing weights. If no function is
        /// given, a default implementation is used (returns <see cref="IReadOnlyEdge{TNodeId, TEdgeId}.Weight"/>).
        /// </remarks>
        /// <param name="graph"><see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> to search.</param>
        /// <param name="startNode">Source <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> for searched paths.</param>
        /// <param name="endNode">
        /// Ending <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> of the path or <c>null</c> if paths to all other nodes
        /// are required.
        /// </param>
        /// <param name="weightFunction">
        /// Function for computing weight of an <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/>. If <c>null</c>, a default
        /// implementation will be used (see remarks).
        /// </param>
        /// <returns>
        /// <see cref="IDictionary{TKey, TValue}"/> with <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> as key and
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
        private static IDictionary<IReadOnlyNode<TNodeId, TEdgeId>, Path<TNodeId, TEdgeId>>
            DijkstrasAlgorithm<TNodeId, TEdgeId>(
            this IReadOnlyGraph<TNodeId, TEdgeId> graph,
            IReadOnlyNode<TNodeId, TEdgeId> startNode,
            IReadOnlyNode<TNodeId, TEdgeId> endNode = null,
            Func<IReadOnlyEdge<TNodeId, TEdgeId>, Weight> weightFunction = null
            )
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            // Local functions
            static Path<TNodeId, TEdgeId>.Segment GetDefaultSegment() => new(null, Weight.positiveInfinity);

            static SinglyLinkedList<Path<TNodeId, TEdgeId>.Segment> InitialisePath() => new(GetDefaultSegment());

            IComparer<SinglyLinkedList<Path<TNodeId, TEdgeId>.Segment>> pathComparer =
                Comparer<SinglyLinkedList<Path<TNodeId, TEdgeId>.Segment>>.Create(
                    (l1, l2) => l1.FirstNode.Data.TotalWeight.CompareTo(l2.FirstNode.Data.TotalWeight));

            // Initialisation
            if (weightFunction == null)
                weightFunction = edge => edge.Weight;
            var unsolvedNodes =
                new BinaryHeap<SinglyLinkedList<Path<TNodeId, TEdgeId>.Segment>, IReadOnlyNode<TNodeId, TEdgeId>>(
                    graph.GetNodes(),
                    InitialisePath,
                    pathComparer);
            try
            {
                var startPath = new SinglyLinkedList<Path<TNodeId, TEdgeId>.Segment>(
                    new Path<TNodeId, TEdgeId>.Segment(null, 0.Weight()));
                unsolvedNodes.DecreaseKey(startNode, startPath);
            }
            catch (ValueNotFoundException e)
            {
                throw new ArgumentException($"{nameof(startNode)} is not present in the graph.", nameof(startNode), e);
            }
            var solvedNodes = new Dictionary<IReadOnlyNode<TNodeId, TEdgeId>, Path<TNodeId, TEdgeId>>();

            // Computation part of the algorithm
            while (!unsolvedNodes.IsEmpty
                && unsolvedNodes.PeekMin().Value != endNode
                && unsolvedNodes.PeekMin().Key.FirstNode.Data.TotalWeight < Weight.positiveInfinity
                )
            {
                var (path, node) = unsolvedNodes.ExtractMin();
                solvedNodes.Add(node, new Path<TNodeId, TEdgeId>(path));
                foreach (var edge in node.GetOutEdges())
                {
                    if (solvedNodes.ContainsKey(edge.ToNode))
                        continue;
                    Weight currWeight = unsolvedNodes[edge.ToNode].FirstNode.Data.TotalWeight;
                    Weight newWeight = path.FirstNode.Data.TotalWeight + weightFunction(edge);
                    if (newWeight < currWeight)
                    {
                        try
                        {
                            unsolvedNodes.DecreaseKey(
                                edge.ToNode,
                                path.AddFront(new Path<TNodeId, TEdgeId>.Segment(edge, newWeight)));
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
                    solvedNodes.Add(node, new Path<TNodeId, TEdgeId>(path));
            else
            {
                if (unsolvedNodes.IsEmpty)
                    throw new ArgumentException($"{nameof(endNode)} is not present in the graph.", nameof(endNode));
                var (path, node) = unsolvedNodes.PeekMin();
                Debug.Assert(node == endNode);
                solvedNodes.Add(node, new Path<TNodeId, TEdgeId>(path));
            }
            return solvedNodes;
        }

        #endregion path_algorithms

        #endregion implementation
    }
}
