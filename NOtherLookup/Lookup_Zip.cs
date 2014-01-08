using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TResult> Zip<TKey, TFirst, TSecond, TResult>(
            this ILookup<TKey, TFirst> first, ILookup<TKey, TSecond> second, 
            Func<TFirst, TSecond, TResult> resultSelector, IEqualityComparer<TKey> comparer = null)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return first.Keys(comparer)
                .Select(key => Zip_ValuesForKey(key, first, second, resultSelector, comparer))
                .ToLookup(comparer);
        }

        private static KeyValuePair<TKey, IEnumerable<TResult>> Zip_ValuesForKey<TKey, TFirst, TSecond, TResult>(TKey key, IEnumerable<IGrouping<TKey, TFirst>> first, IEnumerable<IGrouping<TKey, TSecond>> second, Func<TFirst, TSecond, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            using (var iterator1 = first.ValuesForKey(key, comparer).GetEnumerator())
            {
                var values = new List<TResult>();

                using (var iterator2 = second.ValuesForKey(key, comparer).GetEnumerator())
                {
                    while (iterator1.MoveNext() && iterator2.MoveNext())
                    {
                        values.Add(resultSelector(iterator1.Current, iterator2.Current));
                    }
                }

                return Pair.Of(key, values);
            }
        }
    }
}