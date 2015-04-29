using System;
using System.Collections.Generic;
using NCaching.Entries;

namespace NCaching.Caches {
    internal interface ICache<K, V, out E> : IEnumerable<KeyValuePair<K, V>> where E : CacheEntry<V> {
        void AddOrReplace(K key, V value);
        void Remove(K key);
        void Clear();
        void InvalidateEntries(Func<E, bool> invalidate);
    }
}