using System;
using System.Collections.Generic;

namespace DataStructures.Graphs
{
    /// <summary>
    /// Represents a read-only directed graph.
    /// </summary>
    /// <typeparam name="TNodeId">Type of node identifiers in the graph.</typeparam>
    /// <typeparam name="TEdgeId">Type of edge identifiers in the graph.</typeparam>
    public interface IReadOnlyGraph<TNodeId, TEdgeId>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        /// <summary>
        /// Gets the number of nodes in the graph.
        /// </summary>
        int NodeCount { get; }

        /// <summary>
        /// Gets the number of edges in the graph.
        /// </summary>
        int EdgeCount { get; }

        /// <summary>
        /// Gets <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> with a given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the returned node.</param>
        /// <returns>
        /// If the graph has a <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> with <paramref name="id"/>, returns that
        /// node.
        /// Otherwise returns <c>default</c>.
        /// </returns>
        IReadOnlyNode<TNodeId, TEdgeId> GetNode(TNodeId id);

        /// <summary>
        /// Gets <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with a given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the returned edge.</param>
        /// <returns>
        /// If the graph has an <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with <paramref name="id"/>, returns that
        /// edge.
        /// Otherwise returns <c>default</c>.
        /// </returns>
        IReadOnlyEdge<TNodeId, TEdgeId> GetEdge(TEdgeId id);

        /// <summary>
        /// Gets all nodes in the graph.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> of type <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> with all nodes in no
        /// particular order.
        /// </returns>
        IEnumerable<IReadOnlyNode<TNodeId, TEdgeId>> GetNodes();

        /// <summary>
        /// Gets all edges in the graph.
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{T}"/> of type <see cref="IReadOnlyEdge{TNodeId, TEdgeId}"/> with all edges in no
        /// particular order.
        /// </returns>
        IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetEdges();
    }

    /// <summary>
    /// Represents a directed graph.
    /// </summary>
    /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
    public interface IGraph<TNodeId, TEdgeId>
        : IReadOnlyGraph<TNodeId, TEdgeId>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        /// <summary>
        /// Adds <see cref="INode{TNodeId, TEdgeId}"/> to the graph.
        /// </summary>
        /// <param name="node">Added <see cref="INode{TNodeId, TEdgeId}"/>.</param>
        /// <returns><c>true</c> if the node was successfully added, otherwise <c>false</c>.</returns>
        bool AddNode(INode<TNodeId, TEdgeId> node);

        /// <summary>
        /// Adds <see cref="IEdge{TNodeId, TEdgeId}"/> to the graph.
        /// </summary>
        /// <param name="edge">Added <see cref="IEdge{TNodeId, TEdgeId}"/></param>
        /// <returns><c>true</c> if the edge was successfully added, otherwise <c>false</c>.</returns>
        bool AddEdge(IEdge<TNodeId, TEdgeId> edge);

        /// <summary>
        /// Removes <see cref="INode{TNodeId, TEdgeId}"/> with a given <paramref name="id"/> from the graph.
        /// </summary>
        /// <param name="id">ID of the removed <see cref="INode{TNodeId, TEdgeId}"/>.</param>
        /// <returns>
        /// The removed <see cref="INode{TNodeId, TEdgeId}"/>. If no node was removed, returns <c>default</c>.
        /// </returns>
        INode<TNodeId, TEdgeId> RemoveNode(TNodeId id);

        /// <summary>
        /// Removes <see cref="IEdge{TNodeId, TEdgeId}"/> with a given <paramref name="id"/> from the graph.
        /// </summary>
        /// <param name="id">ID of the removed <see cref="IEdge{TNodeId, TEdgeId}"/>.</param>
        /// <returns>
        /// The removed <see cref="IEdge{TNodeId, TEdgeId}"/>. If no edge was removed, returns <c>default</c>.
        /// </returns>
        IEdge<TNodeId, TEdgeId> RemoveEdge(TEdgeId id);

        /// <summary>
        /// Sets weight of <see cref="IEdge{TNodeId, TEdgeId}"/> with a given <paramref name="edgeId"/> to a given
        /// <paramref name="weight"/>.
        /// </summary>
        /// <param name="edgeId">ID of the <see cref="IEdge{TNodeId, TEdgeId}"/>.</param>
        /// <param name="weight">New weight of the <see cref="IEdge{TNodeId, TEdgeId}"/>.</param>
        /// <returns><c>true</c> if the weight was successfully set, otherwise <c>false</c>.</returns>
        bool SetWeight(TEdgeId edgeId, Weight weight);
    }
}
