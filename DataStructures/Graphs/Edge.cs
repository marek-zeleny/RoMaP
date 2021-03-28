using System;
using System.Diagnostics.CodeAnalysis;

namespace DataStructures.Graphs
{
    /// <summary>
    /// Edge in a <see cref="Graph{TNodeId, TEdgeId}"/>.
    /// </summary>
    /// <inheritdoc cref="IEdge{TNodeId, TEdgeId}"/>
    public class Edge<TNodeId, TEdgeId>
        : IEdge<TNodeId, TEdgeId>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        private readonly INode<TNodeId, TEdgeId> fromNode, toNode;

        public TEdgeId Id { get; }
        public IReadOnlyNode<TNodeId, TEdgeId> FromNode { get => fromNode; }
        public IReadOnlyNode<TNodeId, TEdgeId> ToNode { get => toNode; }
        public Weight Weight { get; private set; }

        /// <summary>
        /// Creates a new edge with a given <paramref name="id"/> between nodes <paramref name="from"/> and
        /// <paramref name="to"/>, having the default weight of 1.
        /// </summary>
        /// <param name="id">ID of the node.</param>
        /// <param name="from">Node where the edge begins.</param>
        /// <param name="to">Node where the edge ends.</param>
        public Edge(TEdgeId id, INode<TNodeId, TEdgeId> from, INode<TNodeId, TEdgeId> to)
            : this(id, from, to, 1.Weight()) { }

        /// <summary>
        /// Creates a new edge with a given <paramref name="id"/> between nodes <paramref name="from"/> and
        /// <paramref name="to"/>, having a given <paramref name="weight"/>.
        /// </summary>
        /// <param name="weight">Weight of the edge.</param>
        /// <inheritdoc cref="Edge{TNodeId, TEdgeId}.Edge(TEdgeId, INode{TNodeId, TEdgeId}, INode{TNodeId, TEdgeId})"/>
        public Edge(TEdgeId id, INode<TNodeId, TEdgeId> from, INode<TNodeId, TEdgeId> to, Weight weight)
        {
            Id = id;
            fromNode = from;
            toNode = to;
            Weight = weight;
        }

        public INode<TNodeId, TEdgeId> GetFromNode() => fromNode;

        public INode<TNodeId, TEdgeId> GetToNode() => toNode;

        public void SetWeight(Weight value) => Weight = value;

        public bool Equals(IReadOnlyEdge<TNodeId, TEdgeId> other)
        {
            return ReferenceEquals(this, other);
        }

        public override string ToString() => $"Edge{Id}: {FromNode.Id} -> {ToNode.Id}, W{Weight}";
    }
}
