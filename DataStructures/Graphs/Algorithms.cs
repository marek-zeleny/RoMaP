using System;
using System.Collections.Generic;
using System.Linq;

using DataStructures.Miscellaneous;

namespace DataStructures.Graphs
{
    public static class Algorithms
    {
        public enum GraphType { Acyclic, NonnegativeWeights, General };

        public static IEnumerable<IEdge<TNodeId, TEdgeId>> FindShortestPath<TNodeId, TEdgeId>(
               this IGraph<TNodeId, TEdgeId> graph,
               GraphType graphType,
               INode<TNodeId, TEdgeId> startNode,
               INode<TNodeId, TEdgeId> endNode,
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
            public SinglyLinkedList<IEdge<TNodeId, TEdgeId>> edges;
            public double totalWeight;

            public Path()
            {
                edges = new SinglyLinkedList<IEdge<TNodeId, TEdgeId>>();
                totalWeight = double.PositiveInfinity;
            }

            public Path(SinglyLinkedList<IEdge<TNodeId, TEdgeId>> edges, double totalWeight)
            {
                this.edges = edges;
                this.totalWeight = totalWeight;
            }

            public int CompareTo(Path<TNodeId, TEdgeId> other) => totalWeight.CompareTo(other.totalWeight);

            public override string ToString()
            {
                return string.Format("P - TW{0}", totalWeight);
            }
        }

        private static IEnumerable<IEdge<TNodeId, TEdgeId>> DijkstrasAlgorithm<TNodeId, TEdgeId>(
            this IGraph<TNodeId, TEdgeId> graph,
            INode<TNodeId, TEdgeId> startNode,
            INode<TNodeId, TEdgeId> endNode,
            out double pathWeight)
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            var unsolvedNodes = new BinaryHeap<Path<TNodeId, TEdgeId>, INode<TNodeId, TEdgeId>>(graph.Nodes.Select(pair => pair.Value), () => new Path<TNodeId, TEdgeId>());
            unsolvedNodes.DecreaseKey(startNode, path => path.totalWeight = 0);
            while (!unsolvedNodes.IsEmpty && unsolvedNodes.PeekMin().Value != endNode)
            {
                var nodePair = unsolvedNodes.ExtractMin();
                var path = nodePair.Key;
                foreach (var edgePair in nodePair.Value.OutEdges)
                {
                    var edge = edgePair.Value;
                    double currWeight = unsolvedNodes[edge.ToNode].totalWeight;
                    double newWeight = path.totalWeight + edge.Weight;
                    if (newWeight < currWeight)
                        unsolvedNodes.DecreaseKey(edge.ToNode, currPath => { currPath.edges = path.edges.AddFront(edge); currPath.totalWeight = newWeight; });
                    // There is no check whether edge.ToNode exists in unsolvedNodes, but the algorithm ensures that it does (unless it has negative weights)
                }
            }
            if (unsolvedNodes.IsEmpty)
                throw new ArgumentException("The target node is not present in the given graph.", nameof(endNode));
            pathWeight = unsolvedNodes.PeekMin().Key.totalWeight;
            return unsolvedNodes.PeekMin().Key.edges;
        }
    }
}
