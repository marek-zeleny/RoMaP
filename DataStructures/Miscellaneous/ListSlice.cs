using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.Miscellaneous
{
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

        public ListSlice(IReadOnlyList<T> list, int index)
            : this(list, index, list.Count - index) { }

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

        public ListSlice<T> Slice(int index) => Slice(index, Count - index);

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
        public static ListSlice<T> Slice<T>(this IReadOnlyList<T> list, int index) => new(list, index);

        public static ListSlice<T> Slice<T>(this IReadOnlyList<T> list, int index, int count) =>
            new(list, index, count);
    }
}
