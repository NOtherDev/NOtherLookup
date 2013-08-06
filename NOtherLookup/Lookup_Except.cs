using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        public static ILookup<TKey, TValue> Except<TKey, TValue>(this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            return ExceptImpl(first, second).ToLookup();
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> ExceptImpl<TKey, TValue>(this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second)
        {
            var secondKeys = new HashSet<TKey>(second.Select(x => x.Key));
            foreach (var grouping in first)
                //if (secondKeys.Add(grouping.Key))
                    yield return new KeyValuePair<TKey, IEnumerable<TValue>>(grouping.Key, grouping.Except(second[grouping.Key]));
        }
    }
}