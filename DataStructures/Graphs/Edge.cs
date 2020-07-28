using System;

namespace DataStructures.Graphs
{
    interface IEdge<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        TEdgeId Id { get; }
        INode<TNodeId, TEdgeId> FromNode { get; }
        INode<TNodeId, TEdgeId> ToNode { get; }
        double Weight { get; }
    }

    class Edge<TNodeId, TEdgeId, TData>
        : IEdge<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        public TEdgeId Id { get; }
        public INode<TNodeId, TEdgeId> FromNode { get; }
        public INode<TNodeId, TEdgeId> ToNode { get; }
        public double Weight { get; private set; }

        public Edge(TEdgeId id, INode<TNodeId, TEdgeId> from, INode<TNodeId, TEdgeId> to, double weight)
        {
            Id = id;
            FromNode = from;
            ToNode = to;
            Weight = weight;
        }
    }
}
