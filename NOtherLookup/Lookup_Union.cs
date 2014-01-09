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
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, 
            IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            return UnionImpl(first, second, keyComparer, valueComparer).ToLookup(keyComparer);
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> UnionImpl<TKey, TValue>(
            ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, 
            IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            var firstKeys = first.Keys(keyComparer);
            var secondKeys = second.Keys(keyComparer);

            foreach (var key in firstKeys)
            {
                secondKeys.Remove(key);
                yield return Pair.Of(key, first.ValuesForKey(key, keyComparer).Union(second.ValuesForKey(key, keyComparer), valueComparer));
            }

            foreach (var newKey in secondKeys)
            {
                yield return Pair.Of(newKey, second.ValuesForKey(newKey, keyComparer));
            }
        }
    }
}