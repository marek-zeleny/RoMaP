using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataStructures.Graphs
{
    public interface IGraph<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        ReadOnlyDictionary<TNodeId, INode<TNodeId, TEdgeId>> Nodes { get; }
        ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> Edges { get; }
        bool AddNode(INode<TNodeId, TEdgeId> node);
        bool AddEdge(IEdge<TNodeId, TEdgeId> edge);
        INode<TNodeId, TEdgeId> RemoveNode(TNodeId id, bool force = false);
        IEdge<TNodeId, TEdgeId> RemoveEdge(TEdgeId id);
    }

    public class DirectedGraph<TNodeId, TEdgeId>
        : IGraph<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        private Dictionary<TNodeId, INode<TNodeId, TEdgeId>> nodes;
        private Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> edges;
        public ReadOnlyDictionary<TNodeId, INode<TNodeId, TEdgeId>> Nodes { get; }
        public ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> Edges { get; }

        public DirectedGraph()
        {
            nodes = new Dictionary<TNodeId, INode<TNodeId, TEdgeId>>();
            edges = new Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>();
            Nodes = new ReadOnlyDictionary<TNodeId, INode<TNodeId, TEdgeId>>(nodes);
            Edges = new ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>(edges);
        }

        public bool AddNode(INode<TNodeId, TEdgeId> node)
        {
            if (nodes.ContainsKey(node.Id))
                return false;
            nodes.Add(node.Id, node);
            return true;
        }

        public bool AddEdge(IEdge<TNodeId, TEdgeId> edge)
        {
            if (edges.ContainsKey(edge.Id))
                return false;
            edges.Add(edge.Id, edge);
            edge.FromNode.AddOutEdge(edge);
            edge.ToNode.AddInEdge(edge);
            return true;
        }

        public INode<TNodeId, TEdgeId> RemoveNode(TNodeId id, bool force = false)
        {
            INode<TNodeId, TEdgeId> node;
            if (!nodes.TryGetValue(id, out node))
                return null;
            if (force)
            {
                foreach (var pair in node.InEdges)
                    RemoveEdge(pair.Key);
                foreach (var pair in node.OutEdges)
                    RemoveEdge(pair.Key);
            }
            else if (node.InEdges.Count > 0 || node.OutEdges.Count > 0)
                return null;
            nodes.Remove(id);
            return node;
        }

        public IEdge<TNodeId, TEdgeId> RemoveEdge(TEdgeId id)
        {
            IEdge<TNodeId, TEdgeId> edge;
            if (!edges.TryGetValue(id, out edge))
                return null;
            edge.FromNode.RemoveOutEdge(id);
            edge.ToNode.RemoveInEdge(id);
            edges.Remove(id);
            return edge;
        }
    }
}
