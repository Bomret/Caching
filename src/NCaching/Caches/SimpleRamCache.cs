using System;
using System.Collections.Generic;
using NCaching.Entries;

namespace NCaching.Caches {
    public sealed class SimpleRamCache<TKey, TValue> : RamCacheBase<TKey, TValue, CacheEntry<TValue>> {
        public SimpleRamCache(IEqualityComparer<TKey> keyComparer) : base(keyComparer) {}

        public override void AddOrReplace(TKey key, TValue value) {
            var cacheEntry = CacheEntry.From(value, DateTimeOffset.UtcNow);

            Cache.AddOrUpdate(key,
                _ => cacheEntry,
                (_1, _2) => cacheEntry);
        }
    }
}