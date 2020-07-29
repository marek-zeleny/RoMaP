using System;
using System.Collections.Generic;

namespace DataStructures.Miscellaneous
{
    public interface IHeap<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        bool IsEmpty { get; }
        void Add(TKey key, TValue value);
        KeyValuePair<TKey, TValue> PeekMin();
        KeyValuePair<TKey, TValue> ExtractMin();
    }
}
