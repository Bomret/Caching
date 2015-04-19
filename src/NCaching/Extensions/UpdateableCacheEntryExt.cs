using System;
using NCaching.Entries;

namespace NCaching.Extensions {
    public static class UpdateableCacheEntryExt {
        public static UpdateableCacheEntry<TValue> RunUpdate<TValue>(this UpdateableCacheEntry<TValue> oldEntry) {
            var updatedValue = oldEntry.Update(oldEntry.Value);
            return UpdateableCacheEntry.From(updatedValue, oldEntry.AddedAt, DateTimeOffset.UtcNow,
                oldEntry.Invalidation, oldEntry.Update);
        }
    }
}