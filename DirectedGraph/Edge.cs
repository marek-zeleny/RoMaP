using System;
using System.Collections.Generic;
using System.Text;

namespace DirectedGraph
{
    interface IEdge<TNodeId, TEdgeId>
    {
        TEdgeId Id { get; }
        INode<TNodeId, TEdgeId> FromNode { get; }
        INode<TNodeId, TEdgeId> ToNode { get; }
        int Length { get; }
    }

    class Edge<TNodeId, TEdgeId, TData> : IEdge<TNodeId, TEdgeId>
    {
        public TEdgeId Id { get; }
        public INode<TNodeId, TEdgeId> FromNode { get; }
        public INode<TNodeId, TEdgeId> ToNode { get; }
        public int Length { get; private set; }

        public Edge(TEdgeId id, INode<TNodeId, TEdgeId> from, INode<TNodeId, TEdgeId> to, int length)
        {
            Id = id;
            FromNode = from;
            ToNode = to;
            Length = length;
        }
    }
}
