using System;
using NCaching.Entries;

namespace NCaching.Caches {
    internal interface IInvalidatingCache<K, V> : ICache<K, V, InvalidateableCacheEntry<V>>, IDisposable {
        void AddOrReplace(K key, V value, Func<CacheEntry<V>, bool> invalidation);
    }
}