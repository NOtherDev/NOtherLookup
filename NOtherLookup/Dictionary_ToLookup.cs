using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static class DictionaryExtensions
    {
        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, IEnumerable<TValue>> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, IEnumerable<TValue>, TValue>(comparer);
        }

        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, IList<TValue>, TValue>(comparer);
        }

        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, ICollection<TValue>, TValue>(comparer);
        }

        [Pure]
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, TValue[]> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, TValue[], TValue>(comparer);
        } 
        
        internal static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> dictionary, IEqualityComparer<TKey> comparer = null)
        {
            return dictionary.ToLookup<TKey, IEnumerable<TValue>, TValue>(comparer);
        }

        private static ILookup<TKey, TValue> ToLookup<TKey, TCollection, TValue>(this IEnumerable<KeyValuePair<TKey, TCollection>> dictionary, IEqualityComparer<TKey> comparer = null)
            where TCollection : IEnumerable<TValue>
        {
            return dictionary.Where(x => !Equals(x.Value, default(TCollection)))
                .SelectMany(kv => kv.Value, (kv, v) => new { kv.Key, Value = v })
                .ToLookup(x => x.Key, x => x.Value, comparer);
        }
    }
}