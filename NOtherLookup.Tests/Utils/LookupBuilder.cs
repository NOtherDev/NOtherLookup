using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup.Tests.Utils
{
    public static class LookupBuilder
    {
        public class Builder<TKey, TValue>
        {
            private readonly Dictionary<TKey, IEnumerable<TValue>> _data = new Dictionary<TKey, IEnumerable<TValue>>();

            public Builder(TKey key, params TValue[] values)
            {
                _data[key] = values;
            }

            public Builder<TKey, TValue> WithKey(TKey key, params TValue[] values)
            {
                _data[key] = values;
                return this;
            }

            public ILookup<TKey, TValue> Build()
            {
                return _data.ToLookup();
            }
        }

        public static Builder<TKey, TValue> WithKey<TKey, TValue>(TKey key, params TValue[] values)
        {
            return new Builder<TKey, TValue>(key, values);
        }
    }
}