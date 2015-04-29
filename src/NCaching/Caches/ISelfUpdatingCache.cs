using System;
using System.Threading;
using System.Threading.Tasks;
using NCaching.Entries;

namespace NCaching.Caches {
    internal interface ISelfUpdatingCache<K, V> : ICache<K, V, UpdateableCacheEntry<V>>, IDisposable {
        void AddOrReplace(K key, V value, Func<UpdateableCacheEntry<V>, bool> invalidation,
            Func<V, CancellationToken, Task<V>> update);
    }
}