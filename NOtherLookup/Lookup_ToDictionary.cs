using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static IDictionary<TKey, IEnumerable<TValue>> ToDictionary<TKey, TValue>(
            this ILookup<TKey, TValue> lookup, IEqualityComparer<TKey> comparer = null)
        {
            if (lookup == null)
                throw new ArgumentNullException("lookup");

            var dictionary = new Dictionary<TKey, IEnumerable<TValue>>(comparer);
            foreach (var group in lookup)
            {
                IEnumerable<TValue> values;
                if (!dictionary.TryGetValue(group.Key, out values))
                    values = Enumerable.Empty<TValue>();
                dictionary[group.Key] = values.Concat(group);
            }
            return dictionary;
        }
    }
}