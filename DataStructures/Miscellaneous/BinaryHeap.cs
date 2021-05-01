using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.Miscellaneous
{
    /// <summary>
    /// Binary minimum heap.
    /// </summary>
    /// <inheritdoc cref="IHeap{TKey, TValue}"/>
    public class BinaryHeap<TKey, TValue>
        : IHeap<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : IComparable<TKey>
        where TValue : IEquatable<TValue>
    {
        private List<KeyValuePair<TKey, TValue>> heap;
        private Dictionary<TValue, int> indexer;

        /// <summary>
        /// Gets the key associated with <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// This is an O(1) operation.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">
        /// <paramref name="value"/> is not found in the heap.
        /// </exception>
        public TKey this[TValue value] { get => heap[indexer[value]].Key; }

        public bool IsEmpty { get => heap.Count == 0; }

        /// <summary>
        /// Creates an empty binary heap.
        /// </summary>
        public BinaryHeap()
        {
            heap = new List<KeyValuePair<TKey, TValue>>();
            indexer = new Dictionary<TValue, int>();
        }

        /// <summary>
        /// Creates an empty binary heap with a given initial <paramref name="capacity"/>.
        /// </summary>
        /// <param name="capacity">Initial capacity of the heap.</param>
        public BinaryHeap(int capacity)
        {
            heap = new List<KeyValuePair<TKey, TValue>>(capacity);
            indexer = new Dictionary<TValue, int>(capacity);
        }

        /// <summary>
        /// Creates a binary heap and fills it with given <paramref name="values"/>, givin each value a
        /// <paramref name="defaultKey"/>.
        /// </summary>
        /// <remarks>
        /// This is an O(<c>n</c>) operation where <c>n</c> is the length of <paramref name="values"/>.
        /// </remarks>
        /// <param name="values"><see cref="IEnumerable{T}"/> of values to fill the heap with.</param>
        /// <param name="defaultKey">Default key to associate with all <paramref name="values"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="values"/> contain duplicite elements.
        /// </exception>
        public BinaryHeap(IEnumerable<TValue> values, TKey defaultKey)
        {
            heap = new List<KeyValuePair<TKey, TValue>>(values.Select(
                value => new KeyValuePair<TKey, TValue>(defaultKey, value)
                ));
            indexer = new Dictionary<TValue, int>(heap.Count);
            for (int i = 0; i < heap.Count; i++)
                indexer.Add(heap[i].Value, i);
        }

        /// <summary>
        /// Creates a binary heap and fills it with given <paramref name="values"/>, generating a key associated with
        /// value with the given <paramref name="generateKey"/> function.
        /// </summary>
        /// <remarks>
        /// If <paramref name="generateKey"/> generates keys that are not equal via the <see cref="IEquatable{T}"/> 
        /// interface, the heap will be incorrectly initialised and further behaviour will be undefined.
        /// This is an O(<c>n</c>) operation where <c>n</c> is the length of <paramref name="values"/>.
        /// </remarks>
        /// <param name="values"><see cref="IEnumerable{T}"/> of values to fill the heap with.</param>
        /// <param name="generateKey">
        /// Function that generates keys. It must return keys that are equal when compared with
        /// <see cref="IEquatable{T}.Equals(T)"/>
        /// </param>
        public BinaryHeap(IEnumerable<TValue> values, Func<TKey> generateKey)
        {
            heap = new List<KeyValuePair<TKey, TValue>>(values.Select(
                value => new KeyValuePair<TKey, TValue>(generateKey(), value)
                ));
            indexer = new Dictionary<TValue, int>(heap.Count);
            for (int i = 0; i < heap.Count; i++)
                indexer.Add(heap[i].Value, i);
        }

        /// <inheritdoc cref="IHeap{TKey, TValue}.Add(TKey, TValue)"/>
        /// <remarks>
        /// This is an O(log(<c>n</c>)) operation where <c>n</c> is the size of the heap.
        /// </remarks>
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
                throw new ArgumentException(
                    "An element with the same value already exists in the heap.", nameof(value), e
                    );
            }
        }

        /// <inheritdoc cref="IHeap{TKey, TValue}.PeekMin"/>
        /// <remarks>
        /// This is an O(1) operation.
        /// </remarks>
        public KeyValuePair<TKey, TValue> PeekMin()
        {
            try
            {
                return heap[0];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new InvalidOperationException("The heap is empty.", e);
            }
        }

        /// <inheritdoc cref="IHeap{TKey, TValue}.ExtractMin"/>
        /// <remarks>
        /// This is an O(log(<c>n</c>)) operation where <c>n</c> is the size of the heap.
        /// </remarks>
        public KeyValuePair<TKey, TValue> ExtractMin()
        {
            var output = PeekMin();
            int lastIndex = heap.Count - 1;
            Switch(0, lastIndex);
            heap.RemoveAt(lastIndex);
            indexer.Remove(output.Value);
            BubbleDown(0);
            return output;
        }

        /// <summary>
        /// Decreases key associated with the given <paramref name="value"/> using a <paramref name="decreaseAction"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="TKey"/> must be a reference type for this function to work. If it's a value type, the function
        /// throws an <see cref="InvalidOperationException"/>.
        /// The caller must be certain that <paramref name="decreaseAction"/> doesn't increase the key instead of
        /// decreasing it, as it cannot be checked here.
        /// This is an O(log(<c>n</c>)) operation where <c>n</c> is the size of the heap.
        /// </remarks>
        /// <param name="value">Value to have its key decreased.</param>
        /// <param name="decreaseAction">
        /// Function that decreases the key associated with <paramref name="value"/>.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="TKey"/> cannot be a value type.
        /// </exception>
        /// <exception cref="ValueNotFoundException">
        /// Element with the given value was not found.
        /// </exception>
        public void DecreaseKey(TValue value, Action<TKey> decreaseAction)
        {
            if (typeof(TKey).IsValueType)
                throw new InvalidOperationException($"{nameof(TKey)} cannot be a value type.");
            int index;
            try
            {
                index = indexer[value];
            }
            catch (KeyNotFoundException e)
            {
                throw new ValueNotFoundException(e);
            }
            decreaseAction(heap[index].Key); // Only makes sense if TKey is a reference type
            BubbleUp(index);
        }

        /// <summary>
        /// Decreases key associated with the given <paramref name="value"/>, setting it to <paramref name="newKey"/>.
        /// </summary>
        /// <remarks>
        /// This is an O(log(<c>n</c>)) operation where <c>n</c> is the size of the heap.
        /// </remarks>
        /// <param name="value">Value to have its key decreased.</param>
        /// <param name="newKey">New key to be associated with <paramref name="value"/>.</param>
        /// <exception cref="ValueNotFoundException">
        /// Element with the given value was not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="newKey"/> cannot be greater than the current key.
        /// </exception>
        public void DecreaseKey(TValue value, TKey newKey)
        {
            int index;
            try
            {
                index = indexer[value];
            }
            catch (KeyNotFoundException e)
            {
                throw new ValueNotFoundException(e);
            }
            if (newKey.CompareTo(heap[index].Key) > 0)
                throw new ArgumentException("The new key cannot be greater than the current key.", nameof(newKey));
            heap[index] = new KeyValuePair<TKey, TValue>(newKey, value);
            BubbleUp(index);
        }

        /// <summary>
        /// Bubbles up an element at the given <paramref name="index"/> through the heap structure.
        /// O(log(<c>n</c>))
        /// </summary>
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

        /// <summary>
        /// Bubbles down an element at the given <paramref name="index"/> through the heap structure.
        /// O(log(<c>n</c>))
        /// </summary>
        private void BubbleDown(int index)
        {
            int childIndex = index * 2 + 1;
            bool b1 = heap.Count > childIndex
                && heap[index].Key.CompareTo(heap[childIndex].Key) > 0;
            bool b2 = heap.Count > childIndex + 1
                && heap[index].Key.CompareTo(heap[childIndex + 1].Key) > 0;
            while (b1 || b2)
            {
                if (b2)
                    childIndex++;
                Switch(index, childIndex);
                index = childIndex;
                childIndex = index * 2 + 1;
                b1 = heap.Count > childIndex
                    && heap[index].Key.CompareTo(heap[childIndex].Key) > 0;
                b2 = heap.Count > childIndex + 1
                    && heap[index].Key.CompareTo(heap[childIndex + 1].Key) > 0;
            }
        }

        /// <summary>
        /// Switches places in the heap structure of elements at indices <paramref name="i1"/> and
        /// <paramref name="i2"/>.
        /// O(1)
        /// </summary>
        private void Switch(int i1, int i2)
        {
            var temp = heap[i1];
            heap[i1] = heap[i2];
            heap[i2] = temp;
            indexer[heap[i1].Value] = i1;
            indexer[heap[i2].Value] = i2;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => heap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// The exception that is thrown when the value specified for accessing an element in a collection does not match
    /// any value in the collection.
    /// </summary>
    public class ValueNotFoundException
        : KeyNotFoundException
    {
        /// <inheritdoc cref="KeyNotFoundException()"/>
        public ValueNotFoundException()
            : base()
        {}

        /// <inheritdoc cref="KeyNotFoundException(string?, Exception?)"/>
        public ValueNotFoundException(KeyNotFoundException innerException)
            : base(innerException.Message, innerException)
        {}
    }
}
