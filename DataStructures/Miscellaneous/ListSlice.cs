using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.Miscellaneous
{
    /// <summary>
    /// Read-only slice of a list.
    /// </summary>
    public class ListSlice<T>
        : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> list;
        private readonly int index;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();
                return list[this.index + index];
            }
        }

        public int Count { get; }

        /// <summary>
        /// Creates a new slice of <paramref name="list"/> from <paramref name="index"/> to the end of the list.
        /// </summary>
        /// <param name="list">Original list to create the slice from</param>
        /// <param name="index">Starting index of the slice</param>
        public ListSlice(IReadOnlyList<T> list, int index)
            : this(list, index, list.Count - index) { }

        /// <summary>
        /// Creates a new slice of <paramref name="list"/> starting at <paramref name="index"/> spanning
        /// <paramref name="count"/> elements.
        /// </summary>
        /// <param name="count">Number of elements in the slice</param>
        /// <inheritdoc cref="ListSlice{T}(IReadOnlyList{T}, int)"/>
        public ListSlice(IReadOnlyList<T> list, int index, int count)
        {
            if (index < 0 || index > list.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (index + count > list.Count)
                throw new ArgumentOutOfRangeException(nameof(count));
            this.list = list;
            this.index = index;
            Count = count;
        }

        /// <summary>
        /// Creates a new slice from this slice from <paramref name="index"/> to the end of this slice.
        /// </summary>
        /// <param name="index">Starting index of the new slice</param>
        /// <returns>New slice sharing the original list with the current slice</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is out of range of the original slice.
        /// </exception>
        public ListSlice<T> Slice(int index) => Slice(index, Count - index);

        /// <summary>
        /// Creates a new slice from this slice starting at <paramref name="index"/> spanning <paramref name="count"/>
        /// elements.
        /// </summary>
        /// <param name="count">Number of elements in the new slice</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is out of range of the original slice or <paramref name="count"/> is too high.
        /// </exception>
        /// <inheritdoc cref="Slice(int)"/>
        public ListSlice<T> Slice(int index, int count)
        {
            if (index > Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (index + count > Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new(list, this.index + index, count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = index; i < index + Count; i++)
                yield return list[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class ListSliceExtensions
    {
        /// <inheritdoc cref="ListSlice{T}(IReadOnlyList{T}, int)"/>
        public static ListSlice<T> Slice<T>(this IReadOnlyList<T> list, int index) => new(list, index);

        /// <inheritdoc cref="ListSlice{T}(IReadOnlyList{T}, int, int)"/>
        public static ListSlice<T> Slice<T>(this IReadOnlyList<T> list, int index, int count) =>
            new(list, index, count);
    }
}
