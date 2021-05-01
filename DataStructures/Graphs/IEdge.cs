using System;

namespace DataStructures.Graphs
{
    /// <summary>
    /// Represents a read-only edge in a <see cref="IGraph{TNodeId, TEdgeId}"/>.
    /// </summary>
    /// <inheritdoc cref="IReadOnlyGraph{TNodeId, TEdgeId}"/>
    public interface IReadOnlyEdge<TNodeId, TEdgeId>
        : IEquatable<IReadOnlyEdge<TNodeId, TEdgeId>>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        /// <summary>
        /// Gets ID of the edge.
        /// </summary>
        TEdgeId Id { get; }

        /// <summary>
        /// Gets <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> where the edge begins.
        /// </summary>
        IReadOnlyNode<TNodeId, TEdgeId> FromNode { get; }

        /// <summary>
        /// Gets <see cref="IReadOnlyNode{TNodeId, TEdgeId}"/> where the edge ends.
        /// </summary>
        IReadOnlyNode<TNodeId, TEdgeId> ToNode { get; }

        /// <summary>
        /// Gets weight of the edge.
        /// </summary>
        Weight Weight { get; }
    }

    /// <summary>
    /// Represents an edge in a <see cref="IGraph{TNodeId, TEdgeId}"/>.
    /// </summary>
    /// <inheritdoc cref="IReadOnlyEdge{TNodeId, TEdgeId}"/>
    public interface IEdge<TNodeId, TEdgeId>
        : IReadOnlyEdge<TNodeId, TEdgeId>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        /// <summary>
        /// Gets <see cref="INode{TNodeId, TEdgeId}"/> where the edge begins.
        /// </summary>
        /// <returns>Mutable instance of the node.</returns>
        INode<TNodeId, TEdgeId> GetFromNode();

        /// <summary>
        /// Gets <see cref="INode{TNodeId, TEdgeId}"/> where the edge ends.
        /// </summary>
        /// <returns>Mutable instance of the node.</returns>
        INode<TNodeId, TEdgeId> GetToNode();

        /// <summary>
        /// Sets the edge's weight.
        /// </summary>
        /// <param name="value">The new weight to set.</param>
        void SetWeight(Weight value);
    }

    /// <summary>
    /// Represents weight of an <see cref="IEdge{TNodeId, TEdgeId}"/> in a <see cref="IGraph{TNodeId, TEdgeId}"/>.
    /// </summary>
    public struct Weight : IComparable<Weight>
    {
        public static readonly Weight positiveInfinity = new Weight(double.PositiveInfinity);
        public static readonly Weight negativeInfinity = new Weight(double.NegativeInfinity);

        private readonly double value;

        public Weight(double value)
        {
            this.value = value;
        }

        public static implicit operator double(Weight w) => w.value;

        public static explicit operator Weight(double d) => new Weight(d);

        public static Weight operator +(Weight w1, Weight w2) => new Weight(w1.value + w2.value);

        public static Weight operator -(Weight w) => new Weight(-w.value);

        public static Weight operator -(Weight w1, Weight w2) => new Weight(w1.value - w2.value);

        public override string ToString() => value.ToString();

        public int CompareTo(Weight other) => value.CompareTo(other.value);
    }

    public static class WeightExtensions
    {
        public static Weight Weight(this double d) => new Weight(d);

        public static Weight Weight(this int i) => new Weight(i);
    }
}
