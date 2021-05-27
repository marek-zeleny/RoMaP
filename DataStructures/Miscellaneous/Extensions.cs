using System;
using System.Collections.Generic;

namespace DataStructures.Miscellaneous
{
    /// <summary>
    /// Contains extensions of standard .NET as well as custom data structures.
    /// </summary>
    public static class Extensions
    {
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
