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

    public class InvalidateableCacheEntry<TValue> : CacheEntry<TValue> {
        internal InvalidateableCacheEntry(TValue value, DateTimeOffset timeAdded, DateTimeOffset? timeUpdated,
            Func<CacheEntry<TValue>, bool> invalidation)
            : base(value, timeAdded, timeUpdated) {
            Invalidation = invalidation;
        }

        public Func<CacheEntry<TValue>, bool> Invalidation { get; }
    }
}