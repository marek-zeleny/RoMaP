using System;
using System.Collections.Generic;

namespace DataStructures.Graphs
{
    public interface IReadOnlyGraph<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        int NodeCount { get; }
        int EdgeCount { get; }
        IReadOnlyNode<TNodeId, TEdgeId> GetNode(TNodeId id);
        IReadOnlyEdge<TNodeId, TEdgeId> GetEdge(TEdgeId id);
        IEnumerable<IReadOnlyNode<TNodeId, TEdgeId>> GetNodes();
        IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetEdges();
    }

    public interface IGraph<TNodeId, TEdgeId>
        : IReadOnlyGraph<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        bool AddNode(INode<TNodeId, TEdgeId> node);
        bool AddEdge(IEdge<TNodeId, TEdgeId> edge);
        INode<TNodeId, TEdgeId> RemoveNode(TNodeId id);
        IEdge<TNodeId, TEdgeId> RemoveEdge(TEdgeId id);
    }
}
