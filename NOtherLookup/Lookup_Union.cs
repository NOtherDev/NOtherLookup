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
            IEnumerable<IGrouping<TKey, TValue>> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> comparer)
        {
            var secondKeys = new HashSet<TKey>(second.Select(x => x.Key), comparer);
            foreach (var grouping in first)
            {
                secondKeys.Remove(grouping.Key);
                yield return UnionValuesForKey(grouping, second, comparer);
            }

            foreach (var newKey in secondKeys)
            {
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(newKey, second[newKey]);
            }
        }

        private static KeyValuePair<TKey, IEnumerable<TValue>> UnionValuesForKey<TKey, TValue>(
            IGrouping<TKey, TValue> current, IEnumerable<IGrouping<TKey, TValue>> second, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var secondValues = second.Where(x => comparer.Equals(x.Key, current.Key)).SelectMany(x => x);
            return new KeyValuePair<TKey, IEnumerable<TValue>>(current.Key, current.Union(secondValues));
        }
    }
}