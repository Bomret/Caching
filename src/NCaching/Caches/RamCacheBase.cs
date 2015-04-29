using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NCaching.Entries;
using NCaching.Extensions;

namespace NCaching.Caches {
    public abstract class RamCacheBase<TKey, TValue, TEntry> : ICache<TKey, TValue, TEntry> where TEntry : CacheEntry<TValue> {
        protected readonly ConcurrentDictionary<TKey, TEntry> Cache;

        protected RamCacheBase(IEqualityComparer<TKey> keyComparer) {
            Cache = new ConcurrentDictionary<TKey, TEntry>(keyComparer);
        }

        public abstract void AddOrReplace(TKey key, TValue value);

        public void Remove(TKey key) {
            TEntry _;
            Cache.TryRemove(key, out _);
        }

        public void Clear() {
            Cache.Clear();
        }

        public void InvalidateEntries(Func<TEntry, bool> invalidation) {
            var keysOfInvalidatedEntries = FindInvalidatedEntries(invalidation)
                .Select(kvp => kvp.Key);

            foreach (var key in keysOfInvalidatedEntries) {
                Remove(key);
            }
        }

        protected IEnumerable<KeyValuePair<TKey, TEntry>> FindInvalidatedEntries(
            Func<TEntry, bool> invalidation) {
            return Cache.Where(kvp => invalidation(kvp.Value));
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() {
            return Cache
                .Select(kvp => kvp.MapV(e => e.Value))
                .GetEnumerator();
        }

        public IEnumerator GetEnumerator() {
            return Cache.GetEnumerator();
        }
    }
}