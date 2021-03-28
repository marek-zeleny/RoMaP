using System;

namespace DataStructures.Graphs
{
    /// <summary>
    /// Exception that is thrown when an error related to <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> occures.
    /// </summary>
    public class GraphException<TNodeId, TEdgeId>
        : Exception
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        /// <summary>
        /// Gets <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> that caused the error.
        /// </summary>
        public IReadOnlyGraph<TNodeId, TEdgeId> Graph { get; } = null;

        /// <summary>
        /// Initialises a new exception with Graph set to <c>null</c> and members of base <see cref="Exception"/> set to
        /// default values.
        /// </summary>
        public GraphException()
            : base()
        {}

        /// <summary>
        /// Initialises a new exception with a given <paramref name="message"/> and <paramref name="graph"/>.
        /// </summary>
        /// <param name="graph">Graph that caused the error.</param>
        /// <inheritdoc cref="Exception(string?)"/>
        public GraphException(string message, IReadOnlyGraph<TNodeId, TEdgeId> graph)
            : base(message)
        {
            Graph = graph;
        }

        /// <summary>
        /// Initialises a new exception with a given <paramref name="message"/>, <paramref name="graph"/> and
        /// <paramref name="innerException"/>.
        /// </summary>
        /// <inheritdoc cref="Exception(string?, Exception?)"/>
        /// <inheritdoc cref="GraphException{TNodeId, TEdgeId}(string, IReadOnlyGraph{TNodeId, TEdgeId})"/>
        public GraphException(string message, IReadOnlyGraph<TNodeId, TEdgeId> graph, Exception innerException)
            : base(message, innerException)
        {
            Graph = graph;
        }

        public override string ToString()
        {
            if (Graph != null)
                return $"{base.ToString()}\nGraph: {Graph}";
            else
                return base.ToString();
        }
    }

    /// <summary>
    /// Exception that is thrown when a <see cref="IReadOnlyGraph{TNodeId, TEdgeId}"/> is inconsistent.
    /// </summary>
    public class InconsistentGraphException<TNodeId, TEdgeId>
        : GraphException<TNodeId, TEdgeId>
        where TNodeId : notnull, IEquatable<TNodeId>
        where TEdgeId : notnull, IEquatable<TEdgeId>
    {
        /// <summary>
        /// Gets the node that caused the inconsistency.
        /// </summary>
        public IReadOnlyNode<TNodeId, TEdgeId> InconsistentNode { get; } = null;
        /// <summary>
        /// Gets the edge that caused the inconsistency.
        /// </summary>
        public IReadOnlyEdge<TNodeId, TEdgeId> InconsistentEdge { get; } = null;

        /// <summary>
        /// Initialises a new exception with InconsistentNode and InconsistentEdge set to <c>null</c> and members of
        /// base <see cref="GraphException{TNodeId, TEdgeId}"/> set to default values.
        /// </summary>
        public InconsistentGraphException()
            : base()
        {}

        /// <summary>
        /// Initialises a new exception caused by an inconsistent node with a given <paramref name="message"/>,
        /// <paramref name="graph"/>, <paramref name="inconsistentNode"/> and optionally
        /// <paramref name="innerException"/>.
        /// </summary>
        /// <param name="inconsistentNode">Node that caused the inconsistency.</param>
        /// <inheritdoc
        /// cref="GraphException{TNodeId, TEdgeId}.GraphException(string, IReadOnlyGraph{TNodeId, TEdgeId}, Exception)"
        /// />
        public InconsistentGraphException(
            string message,
            IReadOnlyGraph<TNodeId, TEdgeId> graph,
            IReadOnlyNode<TNodeId, TEdgeId> inconsistentNode,
            Exception innerException = null
            )
            : base(message, graph, innerException)
        {
            InconsistentNode = inconsistentNode;
        }

        /// <summary>
        /// Initialises a new exception caused by an inconsistent edge with a given <paramref name="message"/>,
        /// <paramref name="graph"/>, <paramref name="inconsistentNode"/> and optionally
        /// <paramref name="innerException"/>.
        /// </summary>
        /// <param name="inconsistentEdge">Edge that caused the inconsistency.</param>
        /// <inheritdoc
        /// cref="GraphException{TNodeId, TEdgeId}.GraphException(string, IReadOnlyGraph{TNodeId, TEdgeId}, Exception)"
        /// />
        public InconsistentGraphException(
            string message,
            IReadOnlyGraph<TNodeId, TEdgeId> graph,
            IReadOnlyEdge<TNodeId, TEdgeId> inconsistentEdge,
            Exception innerException = null
            )
            : base(message, graph, innerException)
        {
            InconsistentEdge = inconsistentEdge;
        }

        public override string ToString()
        {
            if (InconsistentNode != null)
                return $"{base.ToString()}\nInconsistent node: {InconsistentNode}";
            if (InconsistentEdge != null)
                return $"{base.ToString()}\nInconsistent edge: {InconsistentEdge}";
            else
                return base.ToString();
        }
    }
}
