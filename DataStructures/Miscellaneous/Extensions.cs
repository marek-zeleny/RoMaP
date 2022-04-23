using System;
using System.Collections.Generic;

namespace DataStructures.Miscellaneous
{
    /// <summary>
    /// Contains extensions of standard .NET as well as custom data structures.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Enumerates elements of <paramref name="list"/> in the reversed order.
        /// </summary>
        /// <remarks>
        /// This extension method is provided to allow lazy evaluation of getting a reversed linked list, which is not
        /// supported by <see cref="System.Linq.Enumerable.Reverse{TSource}(IEnumerable{TSource})"/>. It also avoids
        /// unnecessarily traversing the list twice.
        /// </remarks>
        /// <param name="list">Linked list to be inverted</param>
        /// <returns>Sequence of elements in <paramref name="list"/> in reversed order</returns>
        public static IEnumerable<T> Reverse<T>(this LinkedList<T> list)
        {
            var node = list.Last;
            while (node != null)
            {
                yield return node.Value;
                node = node.Previous;
            }
        }
    }
}
