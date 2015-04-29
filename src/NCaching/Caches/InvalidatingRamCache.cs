using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using NCaching.Entries;
using NCaching.Extensions;

namespace NCaching.Caches {
    public sealed class InvalidatingRamCache<K, V> :
        RamCacheBase<K, V, InvalidateableCacheEntry<V>>, IInvalidatingCache<K, V> {
        private readonly IClock _clock;
        private readonly IDisposable _invalidation;

        public InvalidatingRamCache(IClock clock, IScheduler invalidationScheduler, TimeSpan invalidationInterval,
            IEqualityComparer<K> keyComparer) : base(keyComparer) {
            _clock = clock;
            _invalidation = new SingleAssignmentDisposable {
                Disposable = Observable.Timer(TimeSpan.Zero, invalidationInterval, invalidationScheduler)
                    .Select(_ => Cache.Where(kvp => kvp.Value.Invalidation(kvp.Value)))
                    .SelectMany(ivs => ivs.Select(kvp => kvp.Key))
                    .Subscribe(Remove)
            };
        }

        public override void AddOrReplace(K key, V value) {
            AddOrReplace(key, value, _ => false);
        }

        public void Dispose() {
            _invalidation.Dispose();
        }

        public void AddOrReplace(K key, V value, Func<CacheEntry<V>, bool> invalidation) {
            var cacheEntry = InvalidateableCacheEntry.From(value, DateTimeOffset.UtcNow, invalidation);

            Cache.AddOrUpdate(key, _ => cacheEntry, (_, e) => e.UpdateWith(value, _clock.UtcNow));
        }
    }
}