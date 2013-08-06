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
            if (second == null)
                throw new ArgumentNullException("second");

            return first.Select(x => new KeyValuePair<TKey, IEnumerable<TValue>>(x.Key, x.Except(second[x.Key]))).ToLookup();
        }
    }
}