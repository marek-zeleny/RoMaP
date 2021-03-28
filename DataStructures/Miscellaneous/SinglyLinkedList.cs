using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.Miscellaneous
{
    /// <summary>
    /// Singly linked list.
    /// </summary>
    /// <remarks>
    /// The structure is immutable and has an O(1) prepend operation. This makes it practical for creating branched
    /// tree structures.
    /// </remarks>
    /// <typeparam name="T">Type of data stored in the list.</typeparam>
    public struct SinglyLinkedList<T>
        : IEnumerable<T>
    {
        /// <summary>
        /// Node in a <see cref="SinglyLinkedList{T}"/>.
        /// </summary>
        /// <remarks>
        /// The class is immutable.
        /// </remarks>
        private class Node
        {
            /// <summary>
            /// Gets data stored in the node.
            /// </summary>
            public T Data { get; }

            /// <summary>
            /// Gets the next node in the list.
            /// </summary>
            public Node Next { get; }

            /// <summary>
            /// Creates a new node prepended to a given <paramref name="next"/> node.
            /// </summary>
            /// <param name="data">Data stored in the node.</param>
            /// <param name="next">Next node in the list. If <c>null</c>, the node will be the last in the list.</param>
            public Node(T data, Node next = null)
            {
                Next = next;
                Data = data;
            }

            public override string ToString() => string.Format("Node: {0}", Data);
        }

        private readonly Node firstNode;

        /// <summary>
        /// Creates a new list with one node containing the given <paramref name="data"/>.
        /// </summary>
        /// <param name="data">Data stored in the created node.</param>
        public SinglyLinkedList(T data)
        {
            firstNode = new Node(data);
        }

        /// <summary>
        /// Creates a new list with the given <paramref name="node"/>.
        /// </summary>
        /// <param name="node">Node that initiates the created list.</param>
        private SinglyLinkedList(Node node)
        {
            firstNode = node;
        }

        /// <summary>
        /// Creates a new <see cref="SinglyLinkedList{T}"/> with a new node prepended to the current list.
        /// </summary>
        /// <remarks>
        /// Since <see cref="SinglyLinkedList{T}"/> is immutable, the original list remains untouched and the new list
        /// only containes the new node that is linked to the original list. This is an O(1) operation.
        /// </remarks>
        /// <param name="data">Data stored in the prepended node.</param>
        /// <returns>The created list with <paramref name="data"/> in the first node.</returns>
        public SinglyLinkedList<T> AddFront(T data)
        {
            return new SinglyLinkedList<T>(new Node(data, firstNode));
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (Node curr = firstNode; curr != null; curr = curr.Next)
                yield return curr.Data;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString() => string.Format("LinkedList: First node: {0}", firstNode);
    }
}
