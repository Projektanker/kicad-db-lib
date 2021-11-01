using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Projektanker.Collections
{
    public class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _dictionary;
        private readonly LinkedList<KeyValuePair<TKey, TValue>> _linkedList;

        public OrderedDictionary()
            : this(EqualityComparer<TKey>.Default)
        {
        }

        public OrderedDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>(comparer);
            _linkedList = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        public int Count => _dictionary.Count;

        public virtual bool IsReadOnly => _dictionary.IsReadOnly;

        public ICollection<TKey> Keys => _linkedList.Select(kv => kv.Key).ToArray();

        public ICollection<TValue> Values => _linkedList.Select(kv => kv.Value).ToArray();

        public TValue this[TKey key]
        {
            get => _dictionary[key].Value.Value;
            set
            {
                var item = KeyValuePair.Create(key, value);
                if (_dictionary.TryGetValue(key, out var node))
                {
                    // Already exists
                    node.Value = item;

                }
                else
                {
                    // Add new
                    node = _linkedList.AddLast(item);
                }

                _dictionary[key] = node;

            }
        }

        public void Add(TKey key, TValue value)
        {
            Add(KeyValuePair.Create(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            var node = new LinkedListNode<KeyValuePair<TKey, TValue>>(item);
            _dictionary.Add(item.Key, node);
            _linkedList.AddLast(node);
        }

        public void Clear()
        {
            _linkedList.Clear();
            _dictionary.Clear();
        }
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.TryGetValue(item.Key, out var node)
                && node.Value.Equals(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {

            _linkedList.CopyTo(array, arrayIndex);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return _linkedList.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_linkedList).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            if (!_dictionary.TryGetValue(key, out var node))
            {
                return false;
            }

            _dictionary.Remove(key);
            _linkedList.Remove(node);
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item) && Remove(item.Key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            bool found = _dictionary.TryGetValue(key, out var node);
            value = found ? node.Value.Value : default;
            return found;
        }
    }
}
