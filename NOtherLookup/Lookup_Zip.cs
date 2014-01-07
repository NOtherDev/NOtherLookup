using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class LookupExtensions
    {
        [Pure]
        public static ILookup<TKey, TResult> Zip<TKey, TFirst, TSecond, TResult>(this ILookup<TKey, TFirst> first,
            ILookup<TKey, TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return first.ZipImpl(second, resultSelector).ToLookup();
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TResult>>> ZipImpl<TKey, TFirst, TSecond, TResult>(
            this ILookup<TKey, TFirst> first,
            ILookup<TKey, TSecond> second, 
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            foreach (var grouping in first)
            {
                using (var iterator1 = grouping.GetEnumerator())
                {
                    var key = grouping.Key;
                    var values = new List<TResult>();

                    using (var iterator2 = second[key].GetEnumerator())
                    {
                        while (iterator1.MoveNext() && iterator2.MoveNext())
                        {
                            values.Add(resultSelector(iterator1.Current, iterator2.Current));
                        }
                    }

                    yield return new KeyValuePair<TKey, IEnumerable<TResult>>(key, values);
                }
            }
        }
    }
}