using System;
using System.Collections.Generic;

namespace DataStructures.Graphs
{
    /// <summary>
    /// Represents a read-only node in a <see cref="IGraph{TNodeId, TEdgeId}"/>.
    /// </summary>
    /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
    public interface IReadOnlyNode<TNodeId, TEdgeId>
        : IEquatable<IReadOnlyNode<TNodeId, TEdgeId>>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        /// <summary>
        /// Gets ID of the node.
        /// </summary>
        TNodeId Id { get; }

        /// <summary>
        /// Gets ingoing degree of the node.
        /// </summary>
        int InDegree { get; }

        /// <summary>
        /// Gets outgoing degree of the node.
        /// </summary>
        int OutDegree { get; }

        /// <summary>
        /// Gets ingoing <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> to the node specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the returned <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/>.</param>
        /// <returns>
        /// If the node has an ingoing <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with <paramref name="id"/>, returns
        /// that edge.
        /// Otherwise returns <c>default</c>.
        /// </returns>
        IReadOnlyEdge<TNodeId, TEdgeId> GetInEdge(TEdgeId id);

        /// <summary>
        /// Gets outgoing <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> from the node specified by
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the returned <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/>.</param>
        /// <returns>
        /// If the node has an outgoing <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with <paramref name="id"/>,
        /// returns that edge.
        /// Otherwise returns <c>default</c>.
        /// </returns>
        IReadOnlyEdge<TNodeId, TEdgeId> GetOutEdge(TEdgeId id);

        /// <summary>
        /// Gets all edges ingoing to the node.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> of type <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with ingoing edges in no
        /// particular order.
        /// </returns>
        IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetInEdges();

        /// <summary>
        /// Gets all edges outgoing from the node.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> of type <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with outgoing edges in no
        /// particular order.
        /// </returns>
        IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetOutEdges();
    }

    /// <summary>
    /// Represents a node in a <see cref="IGraph{TNodeId, TEdgeId}"/>.
    /// </summary>
    /// <inheritdoc cref="IReadOnlyNode{TNodeId, TEdgeId}"/>
    public interface INode<TNodeId, TEdgeId>
        : IReadOnlyNode<TNodeId, TEdgeId>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        /// <summary>
        /// Adds an ingoing <see cref="IEdge{TNodeId, TEdgeId}"/> to the node.
        /// </summary>
        /// <param name="edge">Added <see cref="IEdge{TNodeId, TEdgeId}"/>.</param>
        /// <exception cref="ArgumentException">
        /// An <see cref="IEdge{TNodeId, TEdgeId}"/> with the same ID already goes into the node.
        /// </exception>
        void AddInEdge(IEdge<TNodeId, TEdgeId> edge);

        /// <summary>
        /// Adds an outgoing <see cref="IEdge{TNodeId, TEdgeId}"/> to the <see cref="INode{TNodeId, TEdgeId}"/>.
        /// </summary>
        /// <param name="edge">Added <see cref="IEdge{TNodeId, TEdgeId}"/>.</param>
        /// <exception cref="ArgumentException">
        /// An <see cref="IEdge{TNodeId, TEdgeId}"/> with the same ID already goes from the node.
        /// </exception>
        void AddOutEdge(IEdge<TNodeId, TEdgeId> edge);

        /// <summary>
        /// Removes the node's ingoing <see cref="IEdge{TNodeId, TEdgeId}"/> specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the removed <see cref="IEdge{TNodeId, TEdgeId}"/>.</param>
        /// <returns>
        /// If the node has an ingoing edge with <paramref name="id"/> and the edge is successfully removed, returns
        /// <c>true</c>.
        /// Otherwise returns <c>false</c>.
        /// </returns>
        bool RemoveInEdge(TEdgeId id);

        /// <summary>
        /// Removes the node's outgoing edge specified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the removed edge.</param>
        /// <returns>
        /// If the node has an outgoing edge with <paramref name="id"/> and the edge is successfully removed, returns
        /// <c>true</c>.
        /// Otherwise returns <c>false</c>.
        /// </returns>
        bool RemoveOutEdge(TEdgeId id);
    }
}
