using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.Miscellaneous
{
    public struct SinglyLinkedList<T>
        : IEnumerable<T>
    {
        private class Node
        {
            public T Data { get; }
            public Node Next { get; }

            public Node(T data, Node next = null)
            {
                Next = next;
                Data = data;
            }

            public override string ToString() => string.Format("Node: {0}", Data);
        }

        private readonly Node firstNode;

        public SinglyLinkedList(T data)
        {
            firstNode = new Node(data);
        }

        private SinglyLinkedList(Node node)
        {
            firstNode = node;
        }

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
