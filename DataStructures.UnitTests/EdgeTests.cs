using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using DataStructures.Graphs;

namespace DataStructures.UnitTests
{
    [TestClass]
    public class EdgeTests
    {
        private class TestNode<TNodeId, TEdgeId>
            : INode<TNodeId, TEdgeId>
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            public TNodeId Id { get; }

            public int InDegree => throw new NotImplementedException();

            public int OutDegree => throw new NotImplementedException();

            public TestNode(TNodeId id)
            {
                Id = id;
            }

            public void AddInEdge(IEdge<TNodeId, TEdgeId> edge)
            {
                throw new NotImplementedException();
            }

            public void AddOutEdge(IEdge<TNodeId, TEdgeId> edge)
            {
                throw new NotImplementedException();
            }

            public IReadOnlyEdge<TNodeId, TEdgeId> GetInEdge(TEdgeId id)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetInEdges()
            {
                throw new NotImplementedException();
            }

            public IReadOnlyEdge<TNodeId, TEdgeId> GetOutEdge(TEdgeId id)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IReadOnlyEdge<TNodeId, TEdgeId>> GetOutEdges()
            {
                throw new NotImplementedException();
            }

            public bool RemoveInEdge(TEdgeId id)
            {
                throw new NotImplementedException();
            }

            public bool RemoveOutEdge(TEdgeId id)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Constructor_CorrectInitialization()
        {
            // Arrange & Act
            INode<int, int> node1 = new TestNode<int, int>(1);
            INode<int, int> node2 = new TestNode<int, int>(2);
            Edge<int, int> edge1 = new Edge<int, int>(1, node1, node2, 1);
            Edge<int, int> edge2 = new Edge<int, int>(2, node2, node2, -5.3);

            // Assert
            Assert.AreEqual(1, edge1.Id);
            Assert.AreEqual(node1, edge1.FromNode);
            Assert.AreEqual(node2, edge1.ToNode);
            Assert.AreEqual(node1, edge1.GetFromNode());
            Assert.AreEqual(node2, edge1.GetToNode());
            Assert.AreEqual(1, edge1.Weight);

            Assert.AreEqual(2, edge2.Id);
            Assert.AreEqual(node2, edge2.FromNode);
            Assert.AreEqual(node2, edge2.ToNode);
            Assert.AreEqual(node2, edge2.GetFromNode());
            Assert.AreEqual(node2, edge2.GetToNode());
            Assert.AreEqual(-5.3, edge2.Weight);
        }
    }
}
