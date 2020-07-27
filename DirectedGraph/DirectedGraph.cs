using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DirectedGraph
{
    interface IGraph<TNodeId, TEdgeId>
    {
        ReadOnlyDictionary<TNodeId, INode<TNodeId, TEdgeId>> Nodes { get; }
        ReadOnlyDictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> Edges { get; }
        bool AddNode(INode<TNodeId, TEdgeId> node);
        bool AddEdge(IEdge<TNodeId, TEdgeId> edge);
        INode<TNodeId, TEdgeId> RemoveNode(TNodeId id, bool force = false);
        IEdge<TNodeId, TEdgeId> RemoveEdge(TEdgeId id);
    }

    class DirectedGraph : IGraph<int, int>
    {
        private Dictionary<int, INode<int, int>> nodes;
        private Dictionary<int, IEdge<int, int>> edges;
        public ReadOnlyDictionary<int, INode<int, int>> Nodes { get; }
        public ReadOnlyDictionary<int, IEdge<int, int>> Edges { get; }

        public DirectedGraph()
        {
            nodes = new Dictionary<int, INode<int, int>>();
            edges = new Dictionary<int, IEdge<int, int>>();
            Nodes = new ReadOnlyDictionary<int, INode<int, int>>(nodes);
            Edges = new ReadOnlyDictionary<int, IEdge<int, int>>(edges);
        }

        public bool AddNode(INode<int, int> node)
        {
            if (nodes.ContainsKey(node.Id))
                return false;
            nodes.Add(node.Id, node);
            return true;
        }

        public bool AddEdge(IEdge<int, int> edge)
        {
            if (edges.ContainsKey(edge.Id))
                return false;
            edges.Add(edge.Id, edge);
            edge.FromNode.AddOutEdge(edge);
            edge.ToNode.AddInEdge(edge);
            return true;
        }

        public INode<int, int> RemoveNode(int id, bool force = false)
        {
            INode<int, int> node;
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

        public IEdge<int, int> RemoveEdge(int id)
        {
            IEdge<int, int> edge;
            if (!edges.TryGetValue(id, out edge))
                return null;
            edge.FromNode.RemoveOutEdge(id);
            edge.ToNode.RemoveInEdge(id);
            edges.Remove(id);
            return edge;
        }
    }
}
