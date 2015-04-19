using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NCaching.Entries;
using NCaching.Extensions;

namespace NCaching.Caches {
    public sealed class RefreshingRamCache<TKey, TValue> :
        RamCacheBase<TKey, TValue, UpdateableCacheEntry<TValue>>,
        IRefreshingCache<TKey, TValue> {
        private readonly IDisposable _invalidation;

        public RefreshingRamCache(IScheduler invalidationScheduler, TimeSpan invalidationInterval,
            IEqualityComparer<TKey> keyComparer) : base(keyComparer) {
            _invalidation = new SingleAssignmentDisposable {
                Disposable = Observable.Timer(TimeSpan.Zero, invalidationInterval, invalidationScheduler)
                    .SelectMany(_ => Cache.Where(kvp => kvp.Value.Invalidation(kvp.Value)))
                    .Select(kvp => kvp.MapV(v => v.RunUpdate()))
                    .Subscribe(kvp => Cache.AddOrUpdate(kvp.Key, _ => kvp.Value, (_1, _2) => kvp.Value))
            };
        }

        public override void AddOrReplace(TKey key, TValue value) {
            AddOrReplace(key, value, _ => false, _ => value);
        }

        public void Dispose() {
            _invalidation.Dispose();
        }

        public void AddOrReplace(TKey key, TValue value, Func<CacheEntry<TValue>, bool> invalidation,
            Func<TValue, TValue> update) {
            var cacheEntry = UpdateableCacheEntry.From(value, DateTimeOffset.UtcNow, invalidation, update);

            Cache.AddOrUpdate(key, _ => cacheEntry, (k, e) => cacheEntry);
        }
    }
}