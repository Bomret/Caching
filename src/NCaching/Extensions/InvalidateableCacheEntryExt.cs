using NCaching.Entries;

namespace NCaching.Extensions {
    public static class InvalidateableCacheEntryExt {
        public static bool IsInvalidated<TValue>(this InvalidateableCacheEntry<TValue> entry) {
            return entry.Invalidation(entry);
        }
    }
}