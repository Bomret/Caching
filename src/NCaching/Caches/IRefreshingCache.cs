using System;
using NCaching.Entries;

namespace NCaching.Caches {
    internal interface IRefreshingCache<in TKey, TValue> : ICache<TKey, TValue>, IDisposable {
        void AddOrReplace(TKey key, TValue value, Func<CacheEntry<TValue>, bool> invalidation,
            Func<TValue, TValue> update);
    }
}