using System;

namespace DataStructures.Graphs
{
    public interface IReadOnlyEdge<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        TEdgeId Id { get; }
        IReadOnlyNode<TNodeId, TEdgeId> FromNode { get; }
        IReadOnlyNode<TNodeId, TEdgeId> ToNode { get; }
        double Weight { get; }
    }

    public interface IEdge<TNodeId, TEdgeId>
        : IReadOnlyEdge<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        INode<TNodeId, TEdgeId> GetFromNode();
        INode<TNodeId, TEdgeId> GetToNode();
    }

}
