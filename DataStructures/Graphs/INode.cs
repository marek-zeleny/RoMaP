using System;
using System.Collections.Generic;

namespace DataStructures.Graphs
{
    public interface IReadOnlyNode<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        TNodeId Id { get; }
        int InDegree { get; }
        int OutDegree { get; }
        IReadOnlyEdge<TNodeId, TEdgeId> GetInEdge(TEdgeId id);
        IReadOnlyEdge<TNodeId, TEdgeId> GetOutEdge(TEdgeId id);
        IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetInEdges();
        IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetOutEdges();
    }

    public interface INode<TNodeId, TEdgeId>
        : IReadOnlyNode<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        void AddInEdge(IEdge<TNodeId, TEdgeId> edge);
        void AddOutEdge(IEdge<TNodeId, TEdgeId> edge);
        bool RemoveInEdge(TEdgeId id);
        bool RemoveOutEdge(TEdgeId id);
    }
}
