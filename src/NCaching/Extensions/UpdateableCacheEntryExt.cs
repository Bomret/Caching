using System;
using System.Reactive.Linq;
using NCaching.Entries;

namespace NCaching.Extensions {
    public static class UpdateableCacheEntryExt {
        public static UpdateableCacheEntry<V> UpdateWith<V>(this UpdateableCacheEntry<V> entry, V value,
            DateTimeOffset clock) {
            return UpdateableCacheEntry.From(value, entry.AddedAt, clock, entry.Invalidation, entry.Update);
        }

        public static IObservable<UpdateableCacheEntry<V>> RunUpdate<V>(this UpdateableCacheEntry<V> old, IClock clock) {
            return Observable.FromAsync(c => old.Update(old.Value, c))
                .Select(v => new {Value = v, TimeUpdated = clock.UtcNow})
                .Select(t => old.UpdateWith(t.Value, t.TimeUpdated));
        }
    }
}