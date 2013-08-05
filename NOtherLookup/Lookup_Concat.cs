using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        public static ILookup<TKey, TValue> Concat<TKey, TValue>(this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            return ConcatImpl(first, second).ToLookup();
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> ConcatImpl<TKey, TValue>(this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second)
        {
            var secondKeys = new HashSet<TKey>(second.Select(x => x.Key));
            foreach (var grouping in first)
            {
                secondKeys.Remove(grouping.Key);
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(grouping.Key, grouping.Concat(second[grouping.Key]));
            }

            foreach (var newKey in secondKeys)
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(newKey, second[newKey]);
        }
    }
}