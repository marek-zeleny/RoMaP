using Microsoft.VisualStudio.TestTools.UnitTesting;

using DataStructures.Graphs;
using System;

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
            Assert.AreEqual(node1.Id, 1);
            Assert.AreEqual(node1.Data, default);
            Assert.AreEqual(node1.InDegree, 0);
            Assert.AreEqual(node1.OutDegree, 0);

            Assert.AreEqual(node2.Id, 2);
            Assert.AreEqual(node2.Data, "Second node");
            Assert.AreEqual(node2.InDegree, 0);
            Assert.AreEqual(node2.OutDegree, 0);
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
            Assert.AreEqual(resultInDegree1, 0);
            Assert.AreEqual(resultOutDegree1, 0);
            Assert.AreEqual(resultInDegree2, 2);
            Assert.AreEqual(resultOutDegree2, 0);
            Assert.AreEqual(resultInDegree3, 1);
            Assert.AreEqual(resultOutDegree3, 1);
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

            var resultEdge1 = node1.GetInEdge(1);
            var resultEdge2 = node1.GetInEdge(2);

            // Assert
            Assert.AreEqual(node1.InDegree, 2);
            Assert.AreEqual(resultEdge1, edge1);
            Assert.AreEqual(resultEdge2, edge2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddInEdge_DupliciteId_ExpectedArgumentException()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);

            // Act
            node1.AddInEdge(edge1);
            node1.AddInEdge(edge2);
            node1.AddInEdge(edge1);
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
            Assert.AreEqual(resultEdge1, edge1);
            Assert.AreEqual(resultEdge2, default);
            Assert.AreEqual(resultEdge3, default);
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
            Assert.AreEqual(resultInDegree1, 3);
            Assert.AreEqual(resultInDegree2, 2);
            Assert.AreEqual(resultInDegree3, 1);
            Assert.AreEqual(resultEdge1, edge1);
            Assert.AreEqual(resultEdge2, edge2);
            Assert.AreEqual(resultEdge3, default);
            Assert.AreEqual(resultEdge4, default);
            Assert.AreEqual(resultEdge5, edge2);
            Assert.AreEqual(resultEdge6, default);
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
            Assert.AreEqual(resultInDegree1, 1);
            Assert.AreEqual(resultInDegree2, 1);
            Assert.AreEqual(resultEdge1, edge1);
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

            var resultEdge1 = node1.GetOutEdge(1);
            var resultEdge2 = node1.GetOutEdge(2);

            // Assert
            Assert.AreEqual(node1.OutDegree, 2);
            Assert.AreEqual(resultEdge1, edge1);
            Assert.AreEqual(resultEdge2, edge2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOutEdge_DupliciteId_ExpectedArgumentException()
        {
            // Arrange
            Node<int, int, string> node1 = new Node<int, int, string>(1);
            IEdge<int, int> edge1 = new TestEdge<int, int>(1);
            IEdge<int, int> edge2 = new TestEdge<int, int>(2);

            // Act
            node1.AddOutEdge(edge1);
            node1.AddOutEdge(edge2);
            node1.AddOutEdge(edge1);
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
            Assert.AreEqual(resultEdge1, edge1);
            Assert.AreEqual(resultEdge2, default);
            Assert.AreEqual(resultEdge3, default);
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
            Assert.AreEqual(resultOutDegree1, 3);
            Assert.AreEqual(resultOutDegree2, 2);
            Assert.AreEqual(resultOutDegree3, 1);
            Assert.AreEqual(resultEdge1, edge1);
            Assert.AreEqual(resultEdge2, edge2);
            Assert.AreEqual(resultEdge3, default);
            Assert.AreEqual(resultEdge4, default);
            Assert.AreEqual(resultEdge5, edge2);
            Assert.AreEqual(resultEdge6, default);
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
            Assert.AreEqual(resultOutDegree1, 1);
            Assert.AreEqual(resultOutDegree2, 1);
            Assert.AreEqual(resultEdge1, edge1);
        }

        #endregion
    }
}
