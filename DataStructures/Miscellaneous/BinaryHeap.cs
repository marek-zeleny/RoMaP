using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Miscellaneous
{
    public class BinaryHeap<TKey, TValue>
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

        public BinaryHeap(IEnumerable<TValue> values, Func<TKey> generateKey)
        {
            heap = new List<KeyValuePair<TKey, TValue>>(values.Select(value => new KeyValuePair<TKey, TValue>(generateKey(), value)));
            indexer = new Dictionary<TValue, int>(heap.Count);
            for (int i = 0; i < heap.Count; i++)
                indexer.Add(heap[i].Value, i);
        }

        public void Add(TKey key, TValue value)
        {
            try
            {
                int index = heap.Count;
                indexer.Add(value, index);
                heap.Add(new KeyValuePair<TKey, TValue>(key, value));
                BubbleUp(index);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(string.Format("An element with the same value already exists in the {0}.", nameof(BinaryHeap<TKey, TValue>)), nameof(value), e);
            }
        }

        public KeyValuePair<TKey, TValue> PeekMin()
        {
            try
            {
                return heap[0];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new InvalidOperationException(string.Format("The {0} is empty.", nameof(BinaryHeap<TKey, TValue>)), e);
            }
        }

        public KeyValuePair<TKey, TValue> ExtractMin()
        {
            var output = PeekMin();
            int lastIndex = heap.Count - 1;
            Switch(0, lastIndex);
            heap.RemoveAt(lastIndex);
            indexer.Remove(output.Value);
            return output;
        }

        public bool DecreaseKey(TValue value, Action<TKey> decreaseAction)
        {
            if (!indexer.TryGetValue(value, out int index))
                return false;
            decreaseAction(heap[index].Key); // Only makes sense if TValue is a reference type
            BubbleUp(index);
            return true;
        }

        public bool DecreaseKey(TKey newKey, TValue value)
        {
            if (!indexer.TryGetValue(value, out int index))
                return false;
            heap[index] = new KeyValuePair<TKey, TValue>(newKey, value);
            BubbleUp(index);
            return true;
        }

        private void BubbleUp(int index)
        {
            int parentIndex = (index - 1) / 2;
            while (heap[index].Key.CompareTo(heap[parentIndex].Key) < 0)
            {
                Switch(index, parentIndex);
                index = parentIndex;
                parentIndex = (index - 1) / 2;
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
