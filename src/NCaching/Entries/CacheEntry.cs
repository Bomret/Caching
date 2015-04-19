using System;

namespace NCaching.Entries {
    public static class CacheEntry {
        public static CacheEntry<TValue> From<TValue>(TValue value, DateTimeOffset addedAt) {
            return new CacheEntry<TValue>(value, addedAt, null);
        }

        public static CacheEntry<TValue> From<TValue>(TValue value, DateTimeOffset addedAt,
            DateTimeOffset lastUpdatedAt) {
            return new CacheEntry<TValue>(value, addedAt, lastUpdatedAt);
        }
    }

    public class CacheEntry<TValue> {
        internal CacheEntry(TValue value, DateTimeOffset addedAt, DateTimeOffset? lastUpdatedAt) {
            Value = value;
            AddedAt = addedAt;
            LastUpdatedAt = lastUpdatedAt;
        }

        public TValue Value { get; }
        public DateTimeOffset AddedAt { get; }
        public DateTimeOffset? LastUpdatedAt { get; }
    }
}