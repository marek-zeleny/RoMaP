using System;
using System.Collections.Generic;

namespace DataStructures.Miscellaneous
{
    /// <summary>
    /// Represents a minimum heap structure.
    /// </summary>
    /// <remarks>
    /// In a minimum heap structure, <c>value</c> is the stored object and has to be unique, while <c>key</c> associated
    /// with the <c>value</c> is the minimised quantity.
    /// </remarks>
    /// <typeparam name="TKey">Type of keys in the heap.</typeparam>
    /// <typeparam name="TValue">Type of values in the heap.</typeparam>
    public interface IHeap<TKey, TValue>
    {
        /// <summary>
        /// Indicates whether the heap is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Adds an element into the heap.
        /// </summary>
        /// <param name="key">Key of the added element.</param>
        /// <param name="value">Value of the element.</param>
        /// <exception cref="ArgumentException">
        /// An element with the same <paramref name="value"/> already exists in the heap.
        /// </exception>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Gets the element at the top of the heap (with minimal key) without removing it.
        /// </summary>
        /// <returns><see cref="KeyValuePair{TKey, TValue}"/> with key and value of the peeked element.</returns>
        /// <exception cref="InvalidOperationException">
        /// The heap is empty.
        /// </exception>
        KeyValuePair<TKey, TValue> PeekMin();

        /// <summary>
        /// Gets the element at the top of the heap (with minimal key) and removes it from the heap.
        /// </summary>
        /// <returns><see cref="KeyValuePair{TKey, TValue}"/> with key and value of the extracted element.</returns>
        /// <exception cref="InvalidOperationException">
        /// The heap is empty.
        /// </exception>"
        KeyValuePair<TKey, TValue> ExtractMin();
    }
}
