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
            Edge<int, int, string> edge1 = new Edge<int, int, string>(1, node1, node2, 1);
            Edge<int, int, string> edge2 = new Edge<int, int, string>(2, node2, node2, -5.3, "Second edge");

            // Assert
            Assert.AreEqual(edge1.Id, 1);
            Assert.AreEqual(edge1.FromNode, node1);
            Assert.AreEqual(edge1.ToNode, node2);
            Assert.AreEqual(edge1.GetFromNode(), node1);
            Assert.AreEqual(edge1.GetToNode(), node2);
            Assert.AreEqual(edge1.Weight, 1);
            Assert.AreEqual(edge1.Data, default);

            Assert.AreEqual(edge2.Id, 2);
            Assert.AreEqual(edge2.FromNode, node2);
            Assert.AreEqual(edge2.ToNode, node2);
            Assert.AreEqual(edge2.GetFromNode(), node2);
            Assert.AreEqual(edge2.GetToNode(), node2);
            Assert.AreEqual(edge2.Weight, -5,3);
            Assert.AreEqual(edge2.Data, "Second edge");
        }
    }
}
