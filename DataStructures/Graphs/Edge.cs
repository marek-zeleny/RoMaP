﻿using System;

namespace DataStructures.Graphs
{
    public class Edge<TNodeId, TEdgeId>
        : IEdge<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        private INode<TNodeId, TEdgeId> fromNode;
        private INode<TNodeId, TEdgeId> toNode;

        public TEdgeId Id { get; }
        public IReadOnlyNode<TNodeId, TEdgeId> FromNode { get => fromNode; }
        public IReadOnlyNode<TNodeId, TEdgeId> ToNode { get => toNode; }
        public double Weight { get; protected set; }

        public Edge(TEdgeId id, INode<TNodeId, TEdgeId> from, INode<TNodeId, TEdgeId> to, double weight)
        {
            Id = id;
            fromNode = from;
            toNode = to;
            Weight = weight;
        }

        public INode<TNodeId, TEdgeId> GetFromNode() => fromNode;

        public INode<TNodeId, TEdgeId> GetToNode() => toNode;

        public override string ToString() => string.Format("E{0}: In{1}, Out{2}, W{3}, Data: {4}", Id, FromNode.Id, ToNode.Id, Weight);
    }
}
