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
            private readonly IDictionary<TKey, IEnumerable<TValue>> _data = new Dictionary<TKey, IEnumerable<TValue>>();
            
            private bool _hasNullKey;
            private IEnumerable<TValue> _valuesForNullKey;

            internal LookupBuilder()
            {
            }

            public LookupBuilder<TKey, TValue> WithKey(TKey key, IEnumerable<TValue> values)
            {
                if (key == null)
                {
                    _hasNullKey = true;
                    _valuesForNullKey = values;
                }
                else
                    _data[key] = values;
                return this;
            }
        
            public ILookup<TKey, TValue> Build()
            {
                return _data.Concat(NullKeyEntries).ToLookup();
            }

            private IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> NullKeyEntries
            {
                get
                {
                    if (_hasNullKey)
                        yield return new KeyValuePair<TKey, IEnumerable<TValue>>(default(TKey), _valuesForNullKey);
                }
            }
        }
    }
}