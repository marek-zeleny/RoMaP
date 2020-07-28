using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataStructures.Graphs
{
    interface INode<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        TNodeId Id { get; }
        ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> InEdges { get; }
        ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> OutEdges { get; }
        internal void AddInEdge(IEdge<TNodeId, TEdgeId> edge);
        internal void AddOutEdge(IEdge<TNodeId, TEdgeId> edge);
        internal void RemoveInEdge(TEdgeId id);
        internal void RemoveOutEdge(TEdgeId id);
    }

    class Node<TNodeId, TEdgeId, TData>
        : INode<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        private Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> inEdges;
        private Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> outEdges;

        public TNodeId Id { get; }
        public ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> InEdges { get; }
        public ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> OutEdges { get; }

        public Node(TNodeId id)
        {
            Id = id;
            inEdges = new Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>();
            outEdges = new Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>();
            InEdges = new ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>(inEdges);
            OutEdges = new ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>(outEdges);
        }

        void INode<TNodeId, TEdgeId>.AddInEdge(IEdge<TNodeId, TEdgeId> edge)
        {
            inEdges.Add(edge.Id, edge);
        }

        void INode<TNodeId, TEdgeId>.AddOutEdge(IEdge<TNodeId, TEdgeId> edge)
        {
            outEdges.Add(edge.Id, edge);
        }

        void INode<TNodeId, TEdgeId>.RemoveInEdge(TEdgeId id)
        {
            inEdges.Remove(id);
        }

        void INode<TNodeId, TEdgeId>.RemoveOutEdge(TEdgeId id)
        {
            outEdges.Remove(id);
        }
    }
}
