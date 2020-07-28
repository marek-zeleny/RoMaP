using System;

namespace DataStructures.Graphs
{
    public interface IEdge<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        TEdgeId Id { get; }
        INode<TNodeId, TEdgeId> FromNode { get; }
        INode<TNodeId, TEdgeId> ToNode { get; }
        double Weight { get; }
    }

    public class Edge<TNodeId, TEdgeId, TData>
        : IEdge<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        public TEdgeId Id { get; }
        public INode<TNodeId, TEdgeId> FromNode { get; }
        public INode<TNodeId, TEdgeId> ToNode { get; }
        public double Weight { get; private set; }
        public TData Data { get; set; }

        public Edge(TEdgeId id, INode<TNodeId, TEdgeId> from, INode<TNodeId, TEdgeId> to, double weight, TData data = default)
        {
            Id = id;
            FromNode = from;
            ToNode = to;
            Weight = weight;
            Data = data;
        }

        public override string ToString()
        {
            return string.Format("E{0}: In{1}, Out{2}, W{3}, Data: {4}", Id, FromNode.Id, ToNode.Id, Weight, Data);
        }
    }
}
