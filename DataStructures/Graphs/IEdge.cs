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
        Weight Weight { get; }
    }

    public interface IEdge<TNodeId, TEdgeId>
        : IReadOnlyEdge<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        INode<TNodeId, TEdgeId> GetFromNode();
        INode<TNodeId, TEdgeId> GetToNode();
    }

    public struct Weight : IComparable<Weight>
    {
        private double value;

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
