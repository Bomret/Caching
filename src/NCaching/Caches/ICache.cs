using System;
using NCaching.Entries;

namespace NCaching.Caches {
    internal interface ICache<in TKey, TValue> {
        void AddOrReplace(TKey key, TValue value);
        void Remove(TKey key);
        void Clear();
        void InvalidateEntries(Func<CacheEntry<TValue>, bool> invalidate);
    }
}