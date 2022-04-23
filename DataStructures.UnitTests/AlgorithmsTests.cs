using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

using DataStructures.Graphs;

namespace DataStructures.UnitTests
{
    [TestClass]
    public class AlgorithmsTests
    {
        private struct Path
        {
            public IReadOnlyNode<int, int> from;
            public IReadOnlyNode<int, int> to;
            public Weight weight;
            public List<IReadOnlyEdge<int, int>> edges;
        }

        private IReadOnlyGraph<int, int> smallGraph;
        private IReadOnlyList<Path> smallGraphExamplePaths;
        private IReadOnlyGraph<int, int> largeGraph;
        private IReadOnlyList<Path> largeGraphExamplePaths;
        private IReadOnlyNode<int, int> nonExistentNode;

        [TestInitialize]
        public void InitializeGraphs()
        {
            InitialiseSmallGraph();
            InitialiseLargeGraph();
            nonExistentNode = new Node<int, int>(-5);
        }

        [DataTestMethod]
        [DataRow(Algorithms.GraphType.Acyclic)]
        [DataRow(Algorithms.GraphType.NonnegativeWeights)]
        [DataRow(Algorithms.GraphType.General)]
        [ExpectedException(typeof(ArgumentException))]
        public void FindShortestPath_NonexistingStart_ExpectedArgumentException(Algorithms.GraphType graphType)
        {
            var node1 = smallGraph.GetNode(1);
            smallGraph.FindShortestPath(graphType, nonExistentNode, node1);
        }

        [DataTestMethod]
        [DataRow(Algorithms.GraphType.Acyclic)]
        [DataRow(Algorithms.GraphType.NonnegativeWeights)]
        [DataRow(Algorithms.GraphType.General)]
        [ExpectedException(typeof(ArgumentException))]
        public void FindShortestPath_NonexistingEnd_ExpectedArgumentException(Algorithms.GraphType graphType)
        {
            var node1 = smallGraph.GetNode(1);
            smallGraph.FindShortestPath(graphType, node1, nonExistentNode);
        }

        [DataTestMethod]
        [DataRow(Algorithms.GraphType.Acyclic)]
        [DataRow(Algorithms.GraphType.NonnegativeWeights)]
        [DataRow(Algorithms.GraphType.General)]
        [ExpectedException(typeof(ArgumentException))]
        public void FindShortestPaths_NonexistingStart_ExpectedArgumentException(Algorithms.GraphType graphType)
        {
            smallGraph.FindShortestPaths(graphType, nonExistentNode);
        }

        [TestMethod]
        public void FindShortestPath_NonnegativeWeights_SmallGraph()
        {
            FindShortestPath(smallGraph, smallGraphExamplePaths, Algorithms.GraphType.NonnegativeWeights);
        }

        [TestMethod]
        public void FindShortestPath_NonnegativeWeights_LargeGraph()
        {
            FindShortestPath(largeGraph, largeGraphExamplePaths, Algorithms.GraphType.NonnegativeWeights);
        }

        [TestMethod]
        public void FindShortestPaths_NonnegativeWeights_SmallGraph()
        {
            FindShortestPaths(smallGraph, smallGraphExamplePaths, Algorithms.GraphType.NonnegativeWeights);
        }

        [TestMethod]
        public void FindShortestPaths_NonnegativeWeights_LargeGraph()
        {
            FindShortestPaths(largeGraph, largeGraphExamplePaths, Algorithms.GraphType.NonnegativeWeights);
        }

        private static void FindShortestPath(IReadOnlyGraph<int, int> graph, IReadOnlyList<Path> examplePaths,
            Algorithms.GraphType graphType)
        {
            foreach (var exPath in examplePaths)
            {
                // Act
                var path = graph.FindShortestPath(graphType, exPath.from, exPath.to);

                // Assert
                CollectionAssert.AreEqual(exPath.edges, path.GetEdges().ToList());
                Assert.AreEqual(exPath.weight, path.TotalWeight);
            }
        }

        private static void FindShortestPaths(IReadOnlyGraph<int, int> graph, IReadOnlyList<Path> examplePaths,
            Algorithms.GraphType graphType)
        {
            var paths = new Dictionary<
                IReadOnlyNode<int, int>,
                IDictionary<IReadOnlyNode<int, int>, Algorithms.Path<int, int>>
                >(examplePaths.Count);
            foreach (var exPath in examplePaths)
            {
                // Act
                if (!paths.ContainsKey(exPath.from))
                    paths[exPath.from] = graph.FindShortestPaths(graphType, exPath.from);

                // Assert
                var path = paths[exPath.from][exPath.to];
                CollectionAssert.AreEqual(exPath.edges, path.GetEdges().ToList());
                Assert.AreEqual(exPath.weight, path.TotalWeight);
            }
        }

        private void InitialiseSmallGraph()
        {
            IGraph<int, int> graph = new Graph<int, int>();
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

            var path1to3 = new List<IReadOnlyEdge<int, int>> { edge1, edge3 };
            var path2to1 = new List<IReadOnlyEdge<int, int>>();
            var path3to3 = new List<IReadOnlyEdge<int, int>>();

            var examplePaths = new List<Path>(3);
            examplePaths.Add(new Path { from = node1, to = node3, weight = 12.Weight(), edges = path1to3 });
            examplePaths.Add(new Path { from = node2, to = node1, weight = Weight.positiveInfinity, edges = path2to1 });
            examplePaths.Add(new Path { from = node3, to = node3, weight = 0.Weight(), edges = path3to3 });

            smallGraph = graph;
            smallGraphExamplePaths = examplePaths;
        }

        private void InitialiseLargeGraph()
        {
            IGraph<int, int> graph = new Graph<int, int>();
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

            var path2to9 = new List<IReadOnlyEdge<int, int>> { edge3, edge6, edge10, edge16 };
            var examplePaths = new List<Path>(1);
            examplePaths.Add(new Path { from = node2, to = node9, edges = path2to9, weight = 25.Weight() });

            largeGraph = graph;
            largeGraphExamplePaths = examplePaths;
        }
    }
}