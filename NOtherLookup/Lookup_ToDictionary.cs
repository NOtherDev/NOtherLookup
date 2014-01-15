using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(
            this ILookup<TKey, TValue> lookup, IEqualityComparer<TKey> comparer = null)
        {
            if (lookup == null)
                throw new ArgumentNullException("lookup");

            var dictionary = new Dictionary<TKey, List<TValue>>(comparer);
            foreach (var group in lookup)
            {
                List<TValue> values;
                if (!dictionary.TryGetValue(group.Key, out values))
                {
                    values = new List<TValue>();
                    dictionary[group.Key] = values;
                }
                values.AddRange(group);
            }
            return dictionary;
        }
    }
}