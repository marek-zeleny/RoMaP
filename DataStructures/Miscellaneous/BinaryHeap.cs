using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Miscellaneous
{
    interface IHeap<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        bool IsEmpty { get; }
        void Add(TKey key, TValue value);
        KeyValuePair<TKey, TValue> PeekMin();
        KeyValuePair<TKey, TValue> ExtractMin();
    }

    class BinaryHeap<TKey, TValue>
        : IHeap<TKey, TValue>
        where TKey : IComparable<TKey>
    {    
        private List<KeyValuePair<TKey, TValue>> heap;
        private Dictionary<TValue, int> indexer;

        public TKey this[TValue value] { get => heap[indexer[value]].Key; }

        public bool IsEmpty { get => heap.Count == 0; }

        public BinaryHeap()
        {
            heap = new List<KeyValuePair<TKey, TValue>>();
            indexer = new Dictionary<TValue, int>();
        }

        public BinaryHeap(int capacity)
        {
            heap = new List<KeyValuePair<TKey, TValue>>(capacity);
            indexer = new Dictionary<TValue, int>(capacity);
        }

        public BinaryHeap(IEnumerable<TValue> values, TKey defaultKey)
        {
            heap = new List<KeyValuePair<TKey, TValue>>(values.Select(value => new KeyValuePair<TKey, TValue>(defaultKey, value)));
            indexer = new Dictionary<TValue, int>(heap.Count);
            for (int i = 0; i < heap.Count; i++)
                indexer.Add(heap[i].Value, i);
        }

        public void Add(TKey key, TValue value)
        {
            int index = heap.Count;
            indexer.Add(value, index);
            heap.Add(new KeyValuePair<TKey, TValue>(key, value));
            BubbleUp(index);
        }

        public KeyValuePair<TKey, TValue> PeekMin()
        {
            return heap[0];
        }

        public KeyValuePair<TKey, TValue> ExtractMin()
        {
            var output = heap[0];
            int lastIndex = heap.Count - 1;
            Switch(0, lastIndex);
            heap.RemoveAt(lastIndex);
            indexer.Remove(output.Value);
            return output;
        }

        public void DecreaseKey(TValue value, Action<TKey> decreaseAction)
        {
            int index = indexer[value];
            decreaseAction(heap[index].Key); // Only makes sense if TValue is a reference type
            BubbleDown(index);
        }

        public void DecreaseKey(TKey newKey, TValue value)
        {
            int index = indexer[value];
            heap[index] = new KeyValuePair<TKey, TValue>(newKey, value);
            BubbleDown(index);
        }

        private void BubbleUp(int index)
        {
            int parentIndex = (index - 1) / 2;
            while (heap[index].Key.CompareTo(heap[parentIndex].Key) > 0)
            {
                Switch(index, parentIndex);
                index = parentIndex;
                parentIndex = (index - 1) / 2;
            }
        }

        private void BubbleDown(int index)
        {
            int childIndex = index * 2 + 1;
            bool b1 = heap.Count > childIndex
                && heap[index].Key.CompareTo(heap[childIndex].Key) < 0;
            bool b2 = heap.Count > childIndex + 1
                && heap[index].Key.CompareTo(heap[childIndex + 1].Key) < 0;
            while (b1 || b2)
            {
                if (b2)
                    childIndex++;
                Switch(index, childIndex);
                index = childIndex;
                childIndex = index * 2 + 1;
                b1 = heap.Count > childIndex
                    && heap[index].Key.CompareTo(heap[childIndex].Key) < 0;
                b2 = heap.Count > childIndex + 1
                    && heap[index].Key.CompareTo(heap[childIndex + 1].Key) < 0;
            }
        }

        private void Switch(int i1, int i2)
        {
            var temp = heap[i1];
            heap[i1] = heap[i2];
            heap[i2] = temp;
            indexer[heap[i1].Value] = i1;
            indexer[heap[i2].Value] = i2;
        }
    }
}
