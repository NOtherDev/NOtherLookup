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
                return new LookupBuilder<TKey, TValue>(EqualityComparer<TKey>.Default).WithKey(key, values);
            }
            
            public LookupBuilder<TKey> WithComparer<TKey>(IEqualityComparer<TKey> comparer)
            {
                return new LookupBuilder<TKey>(comparer);
            }
        }

        public class LookupBuilder<TKey>
        {
            private readonly IEqualityComparer<TKey> _comparer;

            public LookupBuilder(IEqualityComparer<TKey> comparer)
            {
                _comparer = comparer;
            }

            public LookupBuilder<TKey, TValue> WithKey<TValue>(TKey key, IEnumerable<TValue> values)
            {
                return new LookupBuilder<TKey, TValue>(_comparer).WithKey(key, values);
            }
        }

        public class LookupBuilder<TKey, TValue>
        {
            private readonly Dictionary<TKey, IEnumerable<TValue>> _data;
            
            private bool _hasNullKey;
            private TKey _firstAppearedNullKey;
            private IEnumerable<TValue> _valuesForNullKey;

            internal LookupBuilder(IEqualityComparer<TKey> comparer)
            {
                _data = new Dictionary<TKey, IEnumerable<TValue>>(comparer);
            }

            public LookupBuilder<TKey, TValue> WithKey(TKey key, IEnumerable<TValue> values)
            {
                // ILookup supports null keys, IDictionary does not, so value for null key need to be kept separately.
                // Null literal cannot be used here as TKey may be a value type, in which case Comparer cannot be called properly.
                // But default(TKey) will produce null for every reference type and it may be used safely here.
                if (typeof(TKey).IsClass && _data.Comparer.Equals(key, default(TKey)))
                {
                    // Null key must be also stored, because when some odd comparers make null equal to some non-null value,
                    // the key which appeared first should be used; when non-null key that is equal to null using provided comparer
                    // appear first, it has to be used as a key for all equal-to-null values.
                    if (!_hasNullKey)
                    {
                        _firstAppearedNullKey = key;
                        _hasNullKey = true;
                    }

                    _valuesForNullKey = values;
                }
                else
                {
                    _data[key] = values;
                }

                return this;
            }
            
            public ILookup<TKey, TValue> Build()
            {
                return _data.Concat(NullKeyEntries).ToLookup(_data.Comparer);
            }

            private IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> NullKeyEntries
            {
                get
                {
                    if (_hasNullKey)
                        yield return new KeyValuePair<TKey, IEnumerable<TValue>>(_firstAppearedNullKey, _valuesForNullKey);
                }
            }
        }
    }
}