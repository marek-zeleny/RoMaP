using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Graphs
{
    public class Node<TNodeId, TEdgeId, TData>
        : INode<TNodeId, TEdgeId>
        where TNodeId : IEquatable<TNodeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        private Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> inEdges;
        private Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>> outEdges;

        public TNodeId Id { get; }
        public int InDegree { get => inEdges.Count; }
        public int OutDegree { get => outEdges.Count; }
        public TData Data { get; set; }

        public Node(TNodeId id, TData data = default)
        {
            Id = id;
            inEdges = new Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>();
            outEdges = new Dictionary<TEdgeId, IEdge<TNodeId, TEdgeId>>();
            Data = data;
        }

        public IReadOnlyEdge<TNodeId, TEdgeId> GetInEdge(TEdgeId id) => inEdges.TryGetValue(id, out var edge) ? edge : default;

        public IReadOnlyEdge<TNodeId, TEdgeId> GetOutEdge(TEdgeId id) => outEdges.TryGetValue(id, out var edge) ? edge : default;

        public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetInEdges() => inEdges.Select(pair => pair.Value);

        public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetOutEdges() => outEdges.Select(pair => pair.Value);

        public void AddInEdge(IEdge<TNodeId, TEdgeId> edge)
        {
            try
            {
                inEdges.Add(edge.Id, edge);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(string.Format("A {0} with the same ID already exists as an in edge of this {1}.", nameof(IEdge<TNodeId, TEdgeId>), nameof(Node<TNodeId, TEdgeId, TData>)), nameof(edge), e);
            }
        }

        public void AddOutEdge(IEdge<TNodeId, TEdgeId> edge)
        {
            try
            {
                outEdges.Add(edge.Id, edge);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(string.Format("A {0} with the same ID already exists as an out edge of this {1}.", nameof(IEdge<TNodeId, TEdgeId>), nameof(Node<TNodeId, TEdgeId, TData>)), nameof(edge), e);
            }
        }

        public bool RemoveInEdge(TEdgeId id) => inEdges.Remove(id);

        public bool RemoveOutEdge(TEdgeId id) => outEdges.Remove(id);

        public override string ToString() => string.Format("N{0}: In{1}, Out{2}, Data: {3}", Id, inEdges.Count, outEdges.Count, Data);
    }
}
