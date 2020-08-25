using System;
using System.Collections.Generic;
using System.Linq;

using DataStructures.Miscellaneous;

namespace DataStructures.Graphs
{
    public static class Algorithms
    {
        public enum GraphType { Acyclic, NonnegativeWeights, General };

        public static IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> FindShortestPath<TNodeId, TEdgeId>(
               this IReadOnlyGraph<TNodeId, TEdgeId> graph,
               GraphType graphType,
               IReadOnlyNode<TNodeId, TEdgeId> startNode,
               IReadOnlyNode<TNodeId, TEdgeId> endNode,
               out Weight pathWeight)
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

        private class Path<TNodeId, TEdgeId>
            : IComparable<Path<TNodeId, TEdgeId>>
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            public SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>> edges;
            public Weight totalWeight;

            public Path()
            {
                edges = new SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>>();
                totalWeight = double.PositiveInfinity.Weight();
            }

            public Path(SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>> edges, Weight totalWeight)
            {
                this.edges = edges;
                this.totalWeight = totalWeight;
            }

            public int CompareTo(Path<TNodeId, TEdgeId> other) => totalWeight.CompareTo(other.totalWeight);

            public override string ToString() => string.Format("P - TW{0}", totalWeight);
        }

        private static IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> DijkstrasAlgorithm<TNodeId, TEdgeId>(
            this IReadOnlyGraph<TNodeId, TEdgeId> graph,
            IReadOnlyNode<TNodeId, TEdgeId> startNode,
            IReadOnlyNode<TNodeId, TEdgeId> endNode,
            out Weight pathWeight)
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            var unsolvedNodes = new BinaryHeap<Path<TNodeId, TEdgeId>, IReadOnlyNode<TNodeId, TEdgeId>>(graph.GetNodes(), () => new Path<TNodeId, TEdgeId>());
            var solvedNodes = new HashSet<IReadOnlyNode<TNodeId, TEdgeId>>();
            unsolvedNodes.DecreaseKey(startNode, path => path.totalWeight = 0.Weight());
            while (!unsolvedNodes.IsEmpty
                && unsolvedNodes.PeekMin().Value != endNode
                && unsolvedNodes.PeekMin().Key.totalWeight < double.PositiveInfinity)
            {
                var (path, node) = unsolvedNodes.ExtractMin();
                solvedNodes.Add(node);
                foreach (var edge in node.GetOutEdges())
                {
                    if (solvedNodes.Contains(edge.ToNode))
                        continue;
                    Weight currWeight = unsolvedNodes[edge.ToNode].totalWeight;
                    Weight newWeight = path.totalWeight + edge.Weight;
                    if (newWeight < currWeight)
                        if (!unsolvedNodes.DecreaseKey(edge.ToNode, currPath => { currPath.edges = path.edges.AddFront(edge); currPath.totalWeight = newWeight; }))
                            throw new ArgumentException(string.Format("The given {0} is inconsistent.", nameof(IReadOnlyGraph<TNodeId, TEdgeId>)), nameof(graph));
                }
            }
            if (unsolvedNodes.IsEmpty)
                throw new ArgumentException(string.Format("The target {0} is not present in the given {1}.", nameof(IReadOnlyNode<TNodeId, TEdgeId>), nameof(IReadOnlyGraph<TNodeId, TEdgeId>)), nameof(endNode));
            var (foundPath, _) = unsolvedNodes.PeekMin();
            pathWeight = foundPath.totalWeight;
            return foundPath.edges.Reverse();
        }
    }
}
