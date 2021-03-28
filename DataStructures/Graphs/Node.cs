using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Graphs
{
    /// <summary>
    /// Node in a <see cref="Graph{TNodeId, TEdgeId}"/>.
    /// </summary>
    /// <inheritdoc cref="INode{TNodeId, TEdgeId}"/>
    public class Node<TNodeId, TEdgeId>
        : INode<TNodeId, TEdgeId>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        protected Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> inEdges, outEdges;

        public TNodeId Id { get; }
        public int InDegree { get => inEdges.Count; }
        public int OutDegree { get => outEdges.Count; }

        /// <summary>
        /// Creates a new node with a given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID of the node. Must be unique within a graph.</param>
        public Node(TNodeId id)
        {
            Id = id;
            inEdges = new Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>();
            outEdges = new Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>();
        }

        /// <inheritdoc cref="IReadOnlyNode{TNodeId, TEdgeId}.GetInEdge(TEdgeId)"/>
        /// <remarks><paramref name="id"/> is guaranteed to be unique within ingoing edges to this node.</remarks>
        public IReadOnlyEdge<TNodeId, TEdgeId> GetInEdge(TEdgeId id)
        {
            return inEdges.TryGetValue(id, out var edge) ? edge : default;
        }

        /// <inheritdoc cref="IReadOnlyNode{TNodeId, TEdgeId}.GetOutEdge(TEdgeId)"/>
        /// <remarks><paramref name="id"/> is guaranteed to be unique within outgoing edges from this node.</remarks>
        public IReadOnlyEdge<TNodeId, TEdgeId> GetOutEdge(TEdgeId id)
        {
            return outEdges.TryGetValue(id, out var edge) ? edge : default;
        }

        public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetInEdges()
        {
            return inEdges.Select(pair => pair.Value);
        }

        public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetOutEdges()
        {
            return outEdges.Select(pair => pair.Value);
        }

        /// <inheritdoc cref="INode{TNodeId, TEdgeId}.AddInEdge(IEdge{TNodeId, TEdgeId})"/>
        /// <remarks>ID of the <paramref name="edge"/> must be unique within ingoing edges to this node.</remarks>
        public virtual void AddInEdge(IEdge<TNodeId, TEdgeId> edge)
        {
            try
            {
                inEdges.Add(edge.Id, edge);
            }
            catch (ArgumentException e)
            {
                string message =
                    $"An {nameof(IEdge<TNodeId, TEdgeId>)} with the same ID already exists as an edge ingoing to " +
                    $"this {nameof(Node<TNodeId, TEdgeId>)}.";
                throw new ArgumentException(message, nameof(edge), e);
            }
        }

        /// <inheritdoc cref="INode{TNodeId, TEdgeId}.AddOutEdge(IEdge{TNodeId, TEdgeId})"/>
        /// <remarks>ID of the <paramref name="edge"/> must be unique within outgoing edges from this node.</remarks>
        public virtual void AddOutEdge(IEdge<TNodeId, TEdgeId> edge)
        {
            try
            {
                outEdges.Add(edge.Id, edge);
            }
            catch (ArgumentException e)
            {
                string message =
                    $"An {nameof(IEdge<TNodeId, TEdgeId>)} with the same ID already exists as an edge outgoing from " +
                    $"this {nameof(Node<TNodeId, TEdgeId>)}.";
                throw new ArgumentException(message, nameof(edge), e);
            }
        }

        public virtual bool RemoveInEdge(TEdgeId id) => inEdges.Remove(id);

        public virtual bool RemoveOutEdge(TEdgeId id) => outEdges.Remove(id);

        public bool Equals(IReadOnlyNode<TNodeId, TEdgeId> other)
        {
            return ReferenceEquals(this, other);
        }

        public override string ToString() => $"Node{Id}: In{inEdges.Count}, Out{outEdges.Count}";
    }
}
