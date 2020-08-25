using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using DataStructures.Graphs;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.UnitTests
{
    [TestClass]
    public class AlgorithmsTests
    {
        [TestMethod]
        public void FindShortestPath_NonnegativeWeights_SmallGraph()
        {
            // Arrange
            IGraph<int, int> graph = new DirectedGraph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);
            INode<int, int> node3 = new Node<int, int>(3);

            IEdge<int, int> edge1 = new Edge<int, int>(1, node1, node2, 5.Weight());
            IEdge<int, int> edge2 = new Edge<int, int>(2, node1, node3, 15.Weight());
            IEdge<int, int> edge3 = new Edge<int, int>(3, node2, node3, 7.Weight());

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);

            graph.AddEdge(edge1);
            graph.AddEdge(edge2);
            graph.AddEdge(edge3);

            // Act
            var path1 = graph.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, node1, node3, out Weight pathWeight1);
            var path2 = graph.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, node2, node1, out Weight pathWeight2);
            var path3 = graph.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, node3, node3, out Weight pathWeight3);

            // Assert
            var expectedPath1 = new List<IReadOnlyEdge<int, int>> { edge1, edge3 };
            var expectedPath2 = new List<IReadOnlyEdge<int, int>>();
            var expectedPath3 = new List<IReadOnlyEdge<int, int>>();
            CollectionAssert.AreEqual(expectedPath1, path1.ToList());
            CollectionAssert.AreEqual(expectedPath2, path2.ToList());
            CollectionAssert.AreEqual(expectedPath3, path3.ToList());
            Assert.AreEqual(12.Weight(), pathWeight1);
            Assert.AreEqual(double.PositiveInfinity.Weight(), pathWeight2);
            Assert.AreEqual(0.Weight(), pathWeight3);
        }

        [TestMethod]
        public void FindShortestPath_NonnegativeWeights_LargeGraph()
        {
            // Arrange
            IGraph<int, int> graph = new DirectedGraph<int, int>();
            INode<int, int> node1 = new Node<int, int>(1);
            INode<int, int> node2 = new Node<int, int>(2);
            INode<int, int> node3 = new Node<int, int>(3);
            INode<int, int> node4 = new Node<int, int>(4);
            INode<int, int> node5 = new Node<int, int>(5);
            INode<int, int> node6 = new Node<int, int>(6);
            INode<int, int> node7 = new Node<int, int>(7);
            INode<int, int> node8 = new Node<int, int>(8);
            INode<int, int> node9 = new Node<int, int>(9);

            IEdge<int, int> edge1 = new Edge<int, int>(1, node1, node2, 4.Weight());
            IEdge<int, int> edge2 = new Edge<int, int>(2, node2, node3, 11.Weight());
            IEdge<int, int> edge3 = new Edge<int, int>(3, node2, node4, 9.Weight());
            IEdge<int, int> edge4 = new Edge<int, int>(4, node3, node1, 8.Weight());
            IEdge<int, int> edge5 = new Edge<int, int>(5, node4, node3, 7.Weight());
            IEdge<int, int> edge6 = new Edge<int, int>(6, node4, node5, 2.Weight());
            IEdge<int, int> edge7 = new Edge<int, int>(7, node4, node6, 6.Weight());
            IEdge<int, int> edge8 = new Edge<int, int>(8, node5, node2, 8.Weight());
            IEdge<int, int> edge9 = new Edge<int, int>(9, node5, node7, 7.Weight());
            IEdge<int, int> edge10 = new Edge<int, int>(10, node5, node8, 4.Weight());
            IEdge<int, int> edge11 = new Edge<int, int>(11, node6, node3, 1.Weight());
            IEdge<int, int> edge12 = new Edge<int, int>(12, node6, node5, 5.Weight());
            IEdge<int, int> edge13 = new Edge<int, int>(13, node7, node8, 14.Weight());
            IEdge<int, int> edge14 = new Edge<int, int>(14, node7, node9, 9.Weight());
            IEdge<int, int> edge15 = new Edge<int, int>(15, node8, node6, 2.Weight());
            IEdge<int, int> edge16 = new Edge<int, int>(16, node8, node9, 10.Weight());

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(node3);
            graph.AddNode(node4);
            graph.AddNode(node5);
            graph.AddNode(node6);
            graph.AddNode(node7);
            graph.AddNode(node8);
            graph.AddNode(node9);

            graph.AddEdge(edge1);
            graph.AddEdge(edge2);
            graph.AddEdge(edge3);
            graph.AddEdge(edge4);
            graph.AddEdge(edge5);
            graph.AddEdge(edge6);
            graph.AddEdge(edge7);
            graph.AddEdge(edge8);
            graph.AddEdge(edge9);
            graph.AddEdge(edge10);
            graph.AddEdge(edge11);
            graph.AddEdge(edge12);
            graph.AddEdge(edge13);
            graph.AddEdge(edge14);
            graph.AddEdge(edge15);
            graph.AddEdge(edge16);

            // Act
            var path1 = graph.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, node2, node9, out Weight pathWeight1);

            // Assert
            var expectedPath1 = new List<IReadOnlyEdge<int, int>> { edge3, edge6, edge10, edge16 };
            CollectionAssert.AreEqual(expectedPath1, path1.ToList());
            Assert.AreEqual(25.Weight(), pathWeight1);
        }
    }
}