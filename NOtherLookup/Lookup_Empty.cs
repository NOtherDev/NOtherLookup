using System;
using System.Collections.Generic;
using System.Linq;

namespace NOtherLookup
{
    public static partial class Lookup
    {
        private class EmptyHolder<TKey, TValue>
        {
            public static readonly ILookup<TKey, TValue> Instance = 
                Enumerable.Empty<int>().ToLookup(x => default(TKey), x => default(TValue));
        }

        public static ILookup<TKey, TValue> Empty<TKey, TValue>()
        {
            return EmptyHolder<TKey, TValue>.Instance;
        }
    }
}
