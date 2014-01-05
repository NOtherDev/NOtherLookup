using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NOtherLookup
{
    public class Lookup<TKey, TValue> : ILookup<TKey, TValue>
    {
        private readonly Dictionary<TKey, IEnumerable<TValue>> _data = new Dictionary<TKey, IEnumerable<TValue>>();

        internal Lookup()
        {
        }

        internal Lookup<TKey, TValue> Add(TKey key, IEnumerable<TValue> values)
        {
            if (values != null)
                _data[key] = values;
            return this;
        }
        
        private class Grouping : IGrouping<TKey, TValue>
        {
            private readonly IEnumerable<TValue> _values;

            public Grouping(TKey key, IEnumerable<TValue> values)
            {
                _values = values;
                Key = key;
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return _values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public TKey Key { get; private set; }
        }

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
        {
            return _data.Select(x => new Grouping(x.Key, x.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(TKey key)
        {
            return _data.ContainsKey(key);
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public IEnumerable<TValue> this[TKey key]
        {
            get
            {
                IEnumerable<TValue> values;
                return _data.TryGetValue(key, out values) ? values : Enumerable.Empty<TValue>();
            }
        }
    }
}