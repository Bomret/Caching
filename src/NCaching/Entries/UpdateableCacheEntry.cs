using System;
using System.Threading;
using System.Threading.Tasks;

namespace NCaching.Entries {
    public static class UpdateableCacheEntry {
        public static UpdateableCacheEntry<V> From<V>(V value,
            DateTimeOffset timeAdded,
            Func<UpdateableCacheEntry<V>, bool> invalidation,
            Func<V, CancellationToken, Task<V>> update) {
            return new UpdateableCacheEntry<V>(value, timeAdded, null, invalidation, update);
        }

        public static UpdateableCacheEntry<V> From<V>(V value,
            DateTimeOffset timeAdded, DateTimeOffset? timeUpdated,
            Func<UpdateableCacheEntry<V>, bool> invalidation,
            Func<V, CancellationToken, Task<V>> update) {
            return new UpdateableCacheEntry<V>(value, timeAdded, timeUpdated, invalidation, update);
        }

        public static UpdateableCacheEntry<TValue> From<TValue>(UpdateableCacheEntry<TValue> oldEntry, TValue value, DateTimeOffset timeUpdated) {
            return From(value, oldEntry.AddedAt, DateTimeOffset.UtcNow, oldEntry.Invalidation, oldEntry.Update);
        }
    }

    public sealed class UpdateableCacheEntry<V> : InvalidateableCacheEntry<V> {
        internal UpdateableCacheEntry(V value, DateTimeOffset timeAdded, DateTimeOffset? timeUpdated,
            Func<UpdateableCacheEntry<V>, bool> invalidation, Func<V, CancellationToken, Task<V>> update)
            : base(value, timeAdded, timeUpdated, invalidation) {
            Update = update;
        }

        public Func<V, CancellationToken, Task<V>> Update { get; }
    }
}