using System;

namespace DirectedGraph
{
    interface IEdge<TNodeId, TEdgeId>
    {
        TEdgeId Id { get; }
        INode<TNodeId, TEdgeId> FromNode { get; }
        INode<TNodeId, TEdgeId> ToNode { get; }
        int Weight { get; }
    }

    class Edge<TNodeId, TEdgeId, TData> : IEdge<TNodeId, TEdgeId>
    {
        public TEdgeId Id { get; }
        public INode<TNodeId, TEdgeId> FromNode { get; }
        public INode<TNodeId, TEdgeId> ToNode { get; }
        public int Weight { get; private set; }

        public Edge(TEdgeId id, INode<TNodeId, TEdgeId> from, INode<TNodeId, TEdgeId> to, int weight)
        {
            Id = id;
            FromNode = from;
            ToNode = to;
            Weight = weight;
        }
    }
}
