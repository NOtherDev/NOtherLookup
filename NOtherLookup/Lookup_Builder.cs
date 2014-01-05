using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public static partial class Lookup
    {
        public static LookupBuilder Builder
        {
            get { return new LookupBuilder(); }
        }

        public class LookupBuilder
        {
            internal LookupBuilder()
            {
            }

            public LookupBuilder<TKey, TValue> WithKey<TKey, TValue>(TKey key, IEnumerable<TValue> values)
            {
                return new LookupBuilder<TKey, TValue>().WithKey(key, values);
            }
        }

        public class LookupBuilder<TKey, TValue>
        {
            private readonly Dictionary<TKey, IEnumerable<TValue>> _data = new Dictionary<TKey, IEnumerable<TValue>>();
            
            internal LookupBuilder()
            {
            }

            public LookupBuilder<TKey, TValue> WithKey(TKey key, IEnumerable<TValue> values)
            {
                if (values != null)
                    _data[key] = values;
                return this;
            }
        
            public ILookup<TKey, TValue> Build()
            {
                return _data.ToLookup();
            }
        }
    }
}