using System;
using System.Collections.Generic;

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
               out double pathWeight)
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
            public double totalWeight;

            public Path()
            {
                edges = new SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>>();
                totalWeight = double.PositiveInfinity;
            }

            public Path(SinglyLinkedList<IReadOnlyEdge<TNodeId, TEdgeId>> edges, double totalWeight)
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
            out double pathWeight)
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            var unsolvedNodes = new BinaryHeap<Path<TNodeId, TEdgeId>, IReadOnlyNode<TNodeId, TEdgeId>>(graph.GetNodes(), () => new Path<TNodeId, TEdgeId>());
            unsolvedNodes.DecreaseKey(startNode, path => path.totalWeight = 0);
            while (!unsolvedNodes.IsEmpty && unsolvedNodes.PeekMin().Value != endNode)
            {
                var (path, node) = unsolvedNodes.ExtractMin();
                foreach (var edge in node.GetOutEdges())
                {
                    double currWeight = unsolvedNodes[edge.ToNode].totalWeight;
                    double newWeight = path.totalWeight + edge.Weight;
                    if (newWeight < currWeight)
                        if (!unsolvedNodes.DecreaseKey(edge.ToNode, currPath => { currPath.edges = path.edges.AddFront(edge); currPath.totalWeight = newWeight; }))
                            throw new ArgumentException(string.Format("The given {0} is inconsistent.", nameof(IReadOnlyGraph<TNodeId, TEdgeId>)), nameof(graph));
                    // There is no check whether edge.ToNode exists in unsolvedNodes, but the algorithm ensures that it does (unless it has negative weights)
                }
            }
            if (unsolvedNodes.IsEmpty)
                throw new ArgumentException(string.Format("The target {0} is not present in the given {1}.", nameof(IReadOnlyNode<TNodeId, TEdgeId>), nameof(IReadOnlyGraph<TNodeId, TEdgeId>)), nameof(endNode));
            var (foundPath, _) = unsolvedNodes.PeekMin();
            pathWeight = foundPath.totalWeight;
            return foundPath.edges;
        }
    }
}
