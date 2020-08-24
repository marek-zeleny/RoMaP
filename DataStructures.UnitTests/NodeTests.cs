using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using DataStructures.Graphs;
using System.Linq;

namespace DataStructures.UnitTests
{
    [TestClass]
    public class NodeTests
    {
        private class TestEdge<TNodeId, TEdgeId>
            : IEdge<TNodeId, TEdgeId>
            where TNodeId : IEquatable<TNodeId>
            where TEdgeId : IEquatable<TEdgeId>
        {
            public TEdgeId Id { get; }

            public IReadOnlyNode<TNodeId, TEdgeId> FromNode => throw new NotImplementedException();

            public IReadOnlyNode<TNodeId, TEdgeId> ToNode => throw new NotImplementedException();

            public double Weight => throw new NotImplementedException();

            public TestEdge(TEdgeId id)
            {
                Id = id;
            }

            public INode<TNodeId, TEdgeId> GetFromNode()
            {
                throw new NotImplementedException();
            }

            public INode<TNodeId, TEdgeId> GetToNode()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Constructor_CorrectInitialization()
        {
            // Arrange & Act
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            Node<int, int, string> node2 = new Node<int, int, string>(2, "Second node");

            // Assert
            Assert.AreEqual(1, node1.Id);
            Assert.AreEqual(default, node1.Data);
            Assert.AreEqual(0, node1.InDegree);
            Assert.AreEqual(0, node1.OutDegree);

            Assert.AreEqual(2, node2.Id);
            Assert.AreEqual("Second node", node2.Data);
            Assert.AreEqual(0, node2.InDegree);
            Assert.AreEqual(0, node2.OutDegree);
        }

        [TestMethod]
        public void InDegree_OutDegree()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);

            // Act
            int resultInDegree1 = node1.InDegree;
            int resultOutDegree1 = node1.OutDegree;

            node1.AddInEdge(edge1);
            node1.AddInEdge(edge2);
            int resultInDegree2 = node1.InDegree;
            int resultOutDegree2 = node1.OutDegree;

            node1.AddOutEdge(edge1);
            node1.RemoveInEdge(2);
            int resultInDegree3 = node1.InDegree;
            int resultOutDegree3 = node1.OutDegree;

            // Assert
            Assert.AreEqual(0, resultInDegree1);
            Assert.AreEqual(0, resultOutDegree1);
            Assert.AreEqual(2, resultInDegree2);
            Assert.AreEqual(0, resultOutDegree2);
            Assert.AreEqual(1, resultInDegree3);
            Assert.AreEqual(1, resultOutDegree3);
        }

        #region InEdge methods

        [TestMethod]
        public void AddInEdge_GetInEdge_Successful()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);

            // Act
            node1.AddInEdge(edge1);
            node1.AddInEdge(edge2);

            int resultInDegree = node1.InDegree;
            var resultEdge1 = node1.GetInEdge(1);
            var resultEdge2 = node1.GetInEdge(2);

            // Assert
            Assert.AreEqual(2, resultInDegree);
            Assert.AreEqual(edge1, resultEdge1);
            Assert.AreEqual(edge2, resultEdge2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddInEdge_DupliciteId_ExpectedArgumentException()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);
            IEdge<int, int> edge3 = new TestEdge<int, int>(1);

            // Act
            node1.AddInEdge(edge1);
            node1.AddInEdge(edge2);
            node1.AddInEdge(edge3);
        }

        [TestMethod]
        public void GetInEdge_NonExistentId_ExpectedDefaultValue()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            Node<int, int, string> node2 = new Node<int, int, string>(2);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);

            // Act
            node1.AddInEdge(edge1);

            var resultEdge1 = node1.GetInEdge(1);
            var resultEdge2 = node1.GetInEdge(13);
            var resultEdge3 = node2.GetInEdge(-140);

            // Assert
            Assert.AreEqual(edge1, resultEdge1);
            Assert.AreEqual(default, resultEdge2);
            Assert.AreEqual(default, resultEdge3);
        }

        [TestMethod]
        public void RemoveInEdge_Successful()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);
            IEdge<int, int> edge3 = new TestEdge<int, int>(3);

            // Act
            node1.AddInEdge(edge1);
            node1.AddInEdge(edge2);
            node1.AddInEdge(edge3);
            int resultInDegree1 = node1.InDegree;

            bool result1 = node1.RemoveInEdge(3);
            int resultInDegree2 = node1.InDegree;
            var resultEdge1 = node1.GetInEdge(1);
            var resultEdge2 = node1.GetInEdge(2);
            var resultEdge3 = node1.GetInEdge(3);

            bool result2 = node1.RemoveInEdge(1);
            int resultInDegree3 = node1.InDegree;
            var resultEdge4 = node1.GetInEdge(1);
            var resultEdge5 = node1.GetInEdge(2);
            var resultEdge6 = node1.GetInEdge(3);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(3, resultInDegree1);
            Assert.AreEqual(2, resultInDegree2);
            Assert.AreEqual(1, resultInDegree3);
            Assert.AreEqual(edge1, resultEdge1);
            Assert.AreEqual(edge2, resultEdge2);
            Assert.AreEqual(default, resultEdge3);
            Assert.AreEqual(default, resultEdge4);
            Assert.AreEqual(edge2, resultEdge5);
            Assert.AreEqual(default, resultEdge6);
        }

        [TestMethod]
        public void RemoveInEdge_Unsuccessful()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);

            // Act
            node1.AddInEdge(edge1);
            int resultInDegree1 = node1.InDegree;

            bool result1 = node1.RemoveInEdge(42);
            int resultInDegree2 = node1.InDegree;
            var resultEdge1 = node1.GetInEdge(1);

            // Assert
            Assert.IsFalse(result1);
            Assert.AreEqual(1, resultInDegree1);
            Assert.AreEqual(1, resultInDegree2);
            Assert.AreEqual(edge1, resultEdge1);
        }

        [TestMethod]
        public void GetInEdges()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);
            IEdge<int, int> edge3 = new TestEdge<int, int>(3);

            // Act
            var resultList1 = node1.GetInEdges().ToList();

            node1.AddInEdge(edge1);
            node1.AddInEdge(edge2);
            node1.AddInEdge(edge3);
            var resultList2 = node1.GetInEdges().ToList();

            node1.RemoveInEdge(edge2.Id);
            var resultList3 = node1.GetInEdges().ToList();

            // Assert
            Assert.AreEqual(0, resultList1.Count);
            Assert.AreEqual(3, resultList2.Count);
            Assert.AreEqual(2, resultList3.Count);
            CollectionAssert.Contains(resultList2, edge1);
            CollectionAssert.Contains(resultList2, edge2);
            CollectionAssert.Contains(resultList2, edge2);
            CollectionAssert.Contains(resultList3, edge1);
            CollectionAssert.DoesNotContain(resultList3, edge2);
            CollectionAssert.Contains(resultList3, edge3);
        }

        #endregion

        #region OutEdge methods

        [TestMethod]
        public void AddOutEdge_GetOutEdge_Successful()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);

            // Act
            node1.AddOutEdge(edge1);
            node1.AddOutEdge(edge2);

            int resultOutDegree = node1.OutDegree;
            var resultEdge1 = node1.GetOutEdge(1);
            var resultEdge2 = node1.GetOutEdge(2);

            // Assert
            Assert.AreEqual(2, resultOutDegree);
            Assert.AreEqual(edge1, resultEdge1);
            Assert.AreEqual(edge2, resultEdge2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOutEdge_DupliciteId_ExpectedArgumentException()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);
            IEdge<int, int> edge3 = new TestEdge<int, int>(1);

            // Act
            node1.AddOutEdge(edge1);
            node1.AddOutEdge(edge2);
            node1.AddOutEdge(edge3);
        }

        [TestMethod]
        public void GetOutEdge_NonExistentId_ExpectedDefaultValue()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            Node<int, int, string> node2 = new Node<int, int, string>(2);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);

            // Act
            node1.AddOutEdge(edge1);

            var resultEdge1 = node1.GetOutEdge(1);
            var resultEdge2 = node1.GetOutEdge(13);
            var resultEdge3 = node2.GetOutEdge(-140);

            // Assert
            Assert.AreEqual(edge1, resultEdge1);
            Assert.AreEqual(default, resultEdge2);
            Assert.AreEqual(default, resultEdge3);
        }

        [TestMethod]
        public void RemoveOutEdge_Successful()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);
            IEdge<int, int> edge3 = new TestEdge<int, int>(3);

            // Act
            node1.AddOutEdge(edge1);
            node1.AddOutEdge(edge2);
            node1.AddOutEdge(edge3);
            int resultOutDegree1 = node1.OutDegree;

            bool result1 = node1.RemoveOutEdge(3);
            int resultOutDegree2 = node1.OutDegree;
            var resultEdge1 = node1.GetOutEdge(1);
            var resultEdge2 = node1.GetOutEdge(2);
            var resultEdge3 = node1.GetOutEdge(3);

            bool result2 = node1.RemoveOutEdge(1);
            int resultOutDegree3 = node1.OutDegree;
            var resultEdge4 = node1.GetOutEdge(1);
            var resultEdge5 = node1.GetOutEdge(2);
            var resultEdge6 = node1.GetOutEdge(3);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(3, resultOutDegree1);
            Assert.AreEqual(2, resultOutDegree2);
            Assert.AreEqual(1, resultOutDegree3);
            Assert.AreEqual(edge1, resultEdge1);
            Assert.AreEqual(edge2, resultEdge2);
            Assert.AreEqual(default, resultEdge3);
            Assert.AreEqual(default, resultEdge4);
            Assert.AreEqual(edge2, resultEdge5);
            Assert.AreEqual(default, resultEdge6);
        }

        [TestMethod]
        public void RemoveOutEdge_Unsuccessful()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);

            // Act
            node1.AddOutEdge(edge1);
            int resultOutDegree1 = node1.OutDegree;

            bool result1 = node1.RemoveOutEdge(42);
            int resultOutDegree2 = node1.OutDegree;
            var resultEdge1 = node1.GetOutEdge(1);

            // Assert
            Assert.IsFalse(result1);
            Assert.AreEqual(1, resultOutDegree1);
            Assert.AreEqual(1, resultOutDegree2);
            Assert.AreEqual(edge1, resultEdge1);
        }

        [TestMethod]
        public void GetOutEdges()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);
            IEdge<int, int> edge3 = new TestEdge<int, int>(3);

            // Act
            var resultList1 = node1.GetOutEdges().ToList();

            node1.AddOutEdge(edge1);
            node1.AddOutEdge(edge2);
            node1.AddOutEdge(edge3);
            var resultList2 = node1.GetOutEdges().ToList();

            node1.RemoveOutEdge(edge2.Id);
            var resultList3 = node1.GetOutEdges().ToList();

            // Assert
            Assert.AreEqual(0, resultList1.Count);
            Assert.AreEqual(3, resultList2.Count);
            Assert.AreEqual(2, resultList3.Count);
            CollectionAssert.Contains(resultList2, edge1);
            CollectionAssert.Contains(resultList2, edge2);
            CollectionAssert.Contains(resultList2, edge2);
            CollectionAssert.Contains(resultList3, edge1);
            CollectionAssert.DoesNotContain(resultList3, edge2);
            CollectionAssert.Contains(resultList3, edge3);
        }

        #endregion
    }
}
