using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static IDictionary<TKey, IEnumerable<TValue>> ToDictionary<TKey, TValue>(this ILookup<TKey, TValue> lookup)
        {
            if (lookup == null)
                throw new ArgumentNullException("lookup");

            var dictionary = new Dictionary<TKey, IEnumerable<TValue>>();
            foreach (var group in lookup)
                dictionary[group.Key] = group;
            return dictionary;
        }
    }
}