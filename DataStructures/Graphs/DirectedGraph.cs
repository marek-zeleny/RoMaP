using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Graphs
{
    public class DirectedGraph<TNodeId, TEdgeId>
        : IGraph<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        private Dictionary<TNodeId, INode<TNodeId, TEdgeId>> nodes;
        private Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> edges;

        public int NodeCount { get => nodes.Count; }
        public int EdgeCount { get => edges.Count; }

        public DirectedGraph()
        {
            nodes = new Dictionary<TNodeId, INode<TNodeId, TEdgeId>>();
            edges = new Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>();
        }

        public IReadOnlyNode<TNodeId, TEdgeId> GetNode(TNodeId id) => nodes.TryGetValue(id, out var node) ? node : default;

        public IReadOnlyEdge<TNodeId, TEdgeId> GetEdge(TEdgeId id) => edges.TryGetValue(id, out var edge) ? edge : default;

        public IEnumerable<IReadOnlyNode<TNodeId, TEdgeId>> GetNodes() => nodes.Select(pair => pair.Value);

        public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetEdges() => edges.Select(pair => pair.Value);

        public bool AddNode(INode<TNodeId, TEdgeId> node)
        {
            if (nodes.ContainsKey(node.Id))
                return false;
            nodes.Add(node.Id, node);
            return true;
        }

        public bool AddEdge(IEdge<TNodeId, TEdgeId> edge)
        {
            bool idIsFree = !edges.ContainsKey(edge.Id);
            bool fromNodeExists = nodes.TryGetValue(edge.FromNode.Id, out var fromNode) && fromNode == edge.FromNode;
            bool toNodeExists = nodes.TryGetValue(edge.ToNode.Id, out var toNode) && toNode == edge.ToNode;
            bool idIsFreeInFromNode = fromNode?.GetOutEdge(edge.Id) == default;
            bool idIsFreeInToNode = toNode?.GetInEdge(edge.Id) == default;

            if (idIsFree && fromNodeExists && toNodeExists && idIsFreeInFromNode && idIsFreeInToNode)
            {
                edges.Add(edge.Id, edge);
                edge.GetFromNode().AddOutEdge(edge);
                edge.GetToNode().AddInEdge(edge);
                return true;
            }
            else
                return false;
        }

        public INode<TNodeId, TEdgeId> RemoveNode(TNodeId id)
        {
            INode<TNodeId, TEdgeId> node;
            if (!nodes.TryGetValue(id, out node)
                || node.InDegree > 0
                || node.OutDegree > 0)
                return default;
            if (!nodes.Remove(id))
                return default;
            return node;
        }

        public INode<TNodeId, TEdgeId> RemoveNodeForced(TNodeId id)
        {
            INode<TNodeId, TEdgeId> node;
            if (!nodes.TryGetValue(id, out node))
                return default;
            foreach (var edge in node.GetInEdges())
                RemoveEdge(edge.Id);
            foreach (var edge in node.GetOutEdges())
                RemoveEdge(edge.Id);
            if (!nodes.Remove(id))
                return default;
            return node;
        }

        public IEdge<TNodeId, TEdgeId> RemoveEdge(TEdgeId id)
        {
            IEdge<TNodeId, TEdgeId> edge;
            if (!edges.TryGetValue(id, out edge))
                return default;
            edge.GetFromNode().RemoveOutEdge(id);
            edge.GetToNode().RemoveInEdge(id);
            if (!edges.Remove(id))
                return default;
            return edge;
        }
    }
}
