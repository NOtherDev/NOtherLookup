using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TValue> Except<TKey, TValue>(
            this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> comparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.Select(x => ExceptValuesForKey(x, second, comparer)).ToLookup(comparer);
        }

        private static KeyValuePair<TKey, IEnumerable<TValue>> ExceptValuesForKey<TKey, TValue>(
            IGrouping<TKey, TValue> current, IEnumerable<IGrouping<TKey, TValue>> second, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var excluded = second.Where(x => comparer.Equals(current.Key, x.Key)).SelectMany(x => x);
            return new KeyValuePair<TKey, IEnumerable<TValue>>(current.Key, current.Except(excluded));
        }
    }
}