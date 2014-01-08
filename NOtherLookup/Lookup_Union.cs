using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TValue> Union<TKey, TValue>(
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> comparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            return UnionImpl(first, second, comparer).ToLookup(comparer);
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> UnionImpl<TKey, TValue>(
            ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> comparer)
        {
            var firstKeys = first.Keys(comparer);
            var secondKeys = second.Keys(comparer);

            foreach (var key in firstKeys)
            {
                secondKeys.Remove(key);
                yield return Pair.Of(key, first.ValuesForKey(key, comparer).Union(second.ValuesForKey(key, comparer)));
            }

            foreach (var newKey in secondKeys)
            {
                yield return Pair.Of(newKey, second.ValuesForKey(newKey, comparer));
            }
        }
    }
}