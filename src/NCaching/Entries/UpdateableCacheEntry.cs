using System;

namespace NCaching.Entries {
    public static class UpdateableCacheEntry {
        public static UpdateableCacheEntry<TValue> From<TValue>(TValue value,
            DateTimeOffset timeAdded,
            Func<CacheEntry<TValue>, bool> invalidation,
            Func<TValue, TValue> refresh) {
            return new UpdateableCacheEntry<TValue>(value, timeAdded, null, invalidation, refresh);
        }

        public static UpdateableCacheEntry<TValue> From<TValue>(TValue value,
            DateTimeOffset timeAdded, DateTimeOffset? timeUpdated,
            Func<CacheEntry<TValue>, bool> invalidation,
            Func<TValue, TValue> refresh) {
            return new UpdateableCacheEntry<TValue>(value, timeAdded, timeUpdated, invalidation, refresh);
        }
    }

    public sealed class UpdateableCacheEntry<TValue> : InvalidateableCacheEntry<TValue> {
        internal UpdateableCacheEntry(TValue value, DateTimeOffset timeAdded, DateTimeOffset? timeUpdated,
            Func<CacheEntry<TValue>, bool> invalidation, Func<TValue, TValue> update)
            : base(value, timeAdded, timeUpdated, invalidation) {
            Update = update;
        }

        public Func<TValue, TValue> Update { get; }
    }
}