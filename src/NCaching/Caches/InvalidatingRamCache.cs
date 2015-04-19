using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NCaching.Entries;

namespace NCaching.Caches {
    public sealed class InvalidatingRamCache<TKey, TValue> :
        RamCacheBase<TKey, TValue, InvalidateableCacheEntry<TValue>>, IInvalidatingCache<TKey, TValue> {
        private readonly IDisposable _invalidation;

        public InvalidatingRamCache(IScheduler invalidationScheduler, TimeSpan invalidationInterval,
            IEqualityComparer<TKey> keyComparer) : base(keyComparer) {
            _invalidation = new SingleAssignmentDisposable {
                Disposable = Observable.Timer(TimeSpan.Zero, invalidationInterval, invalidationScheduler)
                    .Select(_ => Cache.Where(kvp => kvp.Value.Invalidation(kvp.Value)))
                    .SelectMany(ivs => ivs.Select(kvp => kvp.Key))
                    .Subscribe(Remove)
            };
        }

        public override void AddOrReplace(TKey key, TValue value) {
            AddOrReplace(key, value, _ => false);
        }

        public void Dispose() {
            _invalidation.Dispose();
        }

        public void AddOrReplace(TKey key, TValue value, Func<CacheEntry<TValue>, bool> invalidation) {
            var cacheEntry = InvalidateableCacheEntry.From(value, DateTimeOffset.UtcNow, invalidation);
            Cache.AddOrUpdate(key, _ => cacheEntry, (_, e) => cacheEntry);
        }
    }
}