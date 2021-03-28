using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using DataStructures.Graphs;

namespace DataStructures.UnitTests
{
    [TestClass]
    public class DirectedGraphTests
    {
        [TestMethod]
        public void AddNode_GetNode_Successful()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);

            // Act
            bool result1 = graph.AddNode(node1);
            bool result2 = graph.AddNode(node2);
            int resultNodeCount = graph.NodeCount;

            var resultNode1 = graph.GetNode(1);
            var resultNode2 = graph.GetNode(2);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(2, resultNodeCount);
            Assert.AreEqual(node1, resultNode1);
            Assert.AreEqual(node2, resultNode2);
        }

        [TestMethod]
        public void AddNode_DupliciteId()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);
            INode<int, int> node3 = new Node<int, int>(1);

            // Act
            bool result1 = graph.AddNode(node1);
            bool result2 = graph.AddNode(node2);
            int resultNodeCount1 = graph.NodeCount;

            bool result3 = graph.AddNode(node3);
            int resultNodeCount2 = graph.NodeCount;

            var resultNode1 = graph.GetNode(1);
            var resultNode2 = graph.GetNode(2);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.AreEqual(2, resultNodeCount1);
            Assert.AreEqual(2, resultNodeCount2);
            Assert.AreEqual(node1, resultNode1);
            Assert.AreEqual(node2, resultNode2);
        }

        [TestMethod]
        public void GetNode_NonExistentId()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);

            // Act
            var resultNode1 = graph.GetNode(13);

            graph.AddNode(node1);
            var resultNode2 = graph.GetNode(1);
            var resultNode3 = graph.GetNode(-140);

            // Assert
            Assert.AreEqual(default, resultNode1);
            Assert.AreEqual(node1, resultNode2);
            Assert.AreEqual(default, resultNode3);
        }

        [TestMethod]
        public void AddEdge_GetEdge_Successful()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);
            IEdge<int, int> edge1 = new Edge<int, int>(1, node1, node2);
            IEdge<int, int> edge2 = new Edge<int, int>(2, node1, node1);

            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);
            int resultEdgeCount1 = graph.EdgeCount;

            bool result1 = graph.AddEdge(edge1);
            bool result2 = graph.AddEdge(edge2);
            int resultEdgeCount2 = graph.EdgeCount;

            var resultEdge1 = graph.GetEdge(1);
            var resultEdge2 = graph.GetEdge(2);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(0, resultEdgeCount1);
            Assert.AreEqual(2, resultEdgeCount2);
            Assert.AreEqual(edge1, resultEdge1);
            Assert.AreEqual(edge2, resultEdge2);

            Assert.AreEqual(2, node1.OutDegree);
            Assert.AreEqual(0, node2.OutDegree);
            Assert.AreEqual(1, node1.InDegree);
            Assert.AreEqual(1, node2.InDegree);

            Assert.AreEqual(edge1, edge1.GetFromNode().GetOutEdge(1));
            Assert.AreEqual(edge1, edge1.GetToNode().GetInEdge(1));
            Assert.AreEqual(edge2, edge2.GetFromNode().GetOutEdge(2));
            Assert.AreEqual(edge2, edge2.GetToNode().GetInEdge(2));
        }

        [TestMethod]
        public void AddEdge_DupliciteId()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);
            IEdge<int, int> edge1 = new Edge<int, int>(1, node1, node2);
            IEdge<int, int> edge2 = new Edge<int, int>(2, node1, node1);
            IEdge<int, int> edge3 = new Edge<int, int>(1, node2, node2);

            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);
            int resultEdgeCount1 = graph.EdgeCount;

            bool result1 = graph.AddEdge(edge1);
            bool result2 = graph.AddEdge(edge2);
            bool result3 = graph.AddEdge(edge3);
            int resultEdgeCount2 = graph.EdgeCount;

            var resultEdge1 = graph.GetEdge(1);
            var resultEdge2 = graph.GetEdge(2);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.AreEqual(0, resultEdgeCount1);
            Assert.AreEqual(2, resultEdgeCount2);
            Assert.AreEqual(edge1, resultEdge1);
            Assert.AreEqual(edge2, resultEdge2);

            Assert.AreEqual(2, node1.OutDegree);
            Assert.AreEqual(0, node2.OutDegree);
            Assert.AreEqual(1, node1.InDegree);
            Assert.AreEqual(1, node2.InDegree);

            Assert.AreNotEqual(edge3, edge3.GetFromNode().GetOutEdge(1));
            Assert.AreNotEqual(edge3, edge3.GetToNode().GetInEdge(1));
        }

        [TestMethod]
        public void AddEdge_MissingNode()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);
            IEdge<int, int> edge1 = new Edge<int, int>(1, node1, node2);
            IEdge<int, int> edge2 = new Edge<int, int>(2, node2, node1);

            // Act
            graph.AddNode(node2);

            bool result1 = graph.AddEdge(edge1);
            bool result2 = graph.AddEdge(edge2);
            int resultEdgeCount = graph.EdgeCount;

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.AreEqual(0, resultEdgeCount);

            Assert.AreEqual(0, node1.OutDegree);
            Assert.AreEqual(0, node2.OutDegree);
            Assert.AreEqual(0, node1.InDegree);
            Assert.AreEqual(0, node2.InDegree);
        }

        [TestMethod]
        public void AddEdge_DifferentNodeWithSameId()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);
            INode<int, int> node3 = new Node<int, int>(1);
            IEdge<int, int> edge1 = new Edge<int, int>(1, node2, node3);
            IEdge<int, int> edge2 = new Edge<int, int>(2, node3, node1);
            IEdge<int, int> edge3 = new Edge<int, int>(1, node1, node2);

            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);

            bool result1 = graph.AddEdge(edge1);
            bool result2 = graph.AddEdge(edge2);
            bool result3 = graph.AddEdge(edge3);
            int resultEdgeCount = graph.EdgeCount;

            var resultEdge1 = graph.GetEdge(1);
            var resultEdge2 = graph.GetEdge(2);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.AreEqual(1, resultEdgeCount);
            Assert.AreEqual(edge3, resultEdge1);
            Assert.AreEqual(default, resultEdge2);

            Assert.AreEqual(1, node1.OutDegree);
            Assert.AreEqual(0, node2.OutDegree);
            Assert.AreEqual(0, node3.OutDegree);
            Assert.AreEqual(0, node1.InDegree);
            Assert.AreEqual(1, node2.InDegree);
            Assert.AreEqual(0, node3.InDegree);
        }

        [TestMethod]
        public void AddEdge_DupliciteIdInNode()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);
            IEdge<int, int> edge1 = new Edge<int, int>(1, node1, node2);
            IEdge<int, int> edge2 = new Edge<int, int>(1, node2, node1);
            IEdge<int, int> edge3 = new Edge<int, int>(1, node1, node1);
            IEdge<int, int> edge4 = new Edge<int, int>(3, node1, node1);

            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);

            node1.AddOutEdge(edge3);
            node1.AddInEdge(edge3);

            bool result1 = graph.AddEdge(edge1);
            bool result2 = graph.AddEdge(edge2);
            bool result3 = graph.AddEdge(edge4);
            int resultEdgeCount = graph.EdgeCount;

            var resultEdge1 = graph.GetEdge(1);
            var resultEdge2 = graph.GetEdge(2);
            var resultEdge3 = graph.GetEdge(3);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.AreEqual(1, resultEdgeCount);
            Assert.AreEqual(default, resultEdge1);
            Assert.AreEqual(default, resultEdge2);
            Assert.AreEqual(edge4, resultEdge3);

            Assert.AreEqual(2, node1.OutDegree);
            Assert.AreEqual(0, node2.OutDegree);
            Assert.AreEqual(2, node1.InDegree);
            Assert.AreEqual(0, node2.InDegree);
        }

        [TestMethod]
        public void RemoveNode_Successful()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);

            // Act
            graph.AddNode(node1);
            graph.AddNode(node2);
            int resultNodeCount1 = graph.NodeCount;

            var resultNode1 = graph.RemoveNode(1);
            var resultNode2 = graph.RemoveNode(2);
            int resultNodeCount2 = graph.NodeCount;

            // Assert
            Assert.AreEqual(2, resultNodeCount1);
            Assert.AreEqual(0, resultNodeCount2);
            Assert.AreEqual(node1, resultNode1);
            Assert.AreEqual(node2, resultNode2);
        }

        [TestMethod]
        public void RemoveNode_NonExistentId()
        {
            // Arrange
            Graph<int, int> graph = new Graph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);

            // Act
            var resultNode1 = graph.RemoveNode(13);

            graph.AddNode(node1);
            var resultNode2 = graph.RemoveNode(1);
            var resultNode3 = graph.RemoveNode(-140);

            // Assert
            Assert.AreEqual(default, resultNode1);
            Assert.AreEqual(node1, resultNode2);
            Assert.AreEqual(default, resultNode3);
        }
    }
}
