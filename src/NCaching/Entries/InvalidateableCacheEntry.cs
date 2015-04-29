using System;

namespace NCaching.Entries {
    public static class InvalidateableCacheEntry {
        public static InvalidateableCacheEntry<TValue> From<TValue>(TValue value,
            DateTimeOffset timeAdded, Func<CacheEntry<TValue>, bool> invalidation) {
            return new InvalidateableCacheEntry<TValue>(value, timeAdded, null, invalidation);
        }

        public static InvalidateableCacheEntry<TValue> From<TValue>(TValue value,
            DateTimeOffset timeAdded, DateTimeOffset timeUpdated, Func<CacheEntry<TValue>, bool> invalidation) {
            return new InvalidateableCacheEntry<TValue>(value, timeAdded, timeUpdated, invalidation);
        }
    }

    public class InvalidateableCacheEntry<V> : CacheEntry<V> {
        internal InvalidateableCacheEntry(V value, DateTimeOffset timeAdded, DateTimeOffset? timeUpdated,
            Func<InvalidateableCacheEntry<V>, bool> invalidation)
            : base(value, timeAdded, timeUpdated) {
            Invalidation = invalidation;
        }

        public Func<InvalidateableCacheEntry<V>, bool> Invalidation { get; }
    }
}