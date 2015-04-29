using System;
using NCaching.Entries;

namespace NCaching.Extensions {
    public static class InvalidateableCacheEntryExt {
        public static bool IsInvalidated<TValue>(this InvalidateableCacheEntry<TValue> entry) {
            return entry.Invalidation(entry);
        }

        public static InvalidateableCacheEntry<V> UpdateWith<V>(this InvalidateableCacheEntry<V> old, V value,
            DateTimeOffset timeUpdated) {
            return InvalidateableCacheEntry.From(value, old.AddedAt, timeUpdated, old.Invalidation);
        }
    }
}