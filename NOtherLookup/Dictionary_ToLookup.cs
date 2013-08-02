using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class DictionaryExtensions
    {
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, IEnumerable<TValue>> dictionary)
        {
            return dictionary.ToLookup<TKey, IEnumerable<TValue>, TValue>();
        } 
        
        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary)
        {
            return dictionary.ToLookup<TKey, IList<TValue>, TValue>();
        }  

        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> dictionary)
        {
            return dictionary.ToLookup<TKey, ICollection<TValue>, TValue>();
        }  

        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IDictionary<TKey, TValue[]> dictionary)
        {
            return dictionary.ToLookup<TKey, TValue[], TValue>();
        }  
        
        internal static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> dictionary)
        {
            return dictionary.ToLookup<TKey, IEnumerable<TValue>, TValue>();
        }

        private static ILookup<TKey, TValue> ToLookup<TKey, TCollection, TValue>(this IEnumerable<KeyValuePair<TKey, TCollection>> dictionary)
            where TCollection : IEnumerable<TValue>
        {
            return dictionary.Where(x => !Equals(x.Value, default(TCollection)))
                .SelectMany(kv => kv.Value, (kv, v) => new { kv.Key, Value = v })
                .ToLookup(x => x.Key, x => x.Value);
        }
    }
}