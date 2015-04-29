using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using NCaching.Entries;
using NCaching.Extensions;

namespace NCaching.Caches {
    public sealed class SelfUpdatingRamCache<K, V> :
        RamCacheBase<K, V, UpdateableCacheEntry<V>>,
        ISelfUpdatingCache<K, V> {
        private readonly IClock _clock;
        private readonly IDisposable _invalidation;

        public SelfUpdatingRamCache(IClock clock, IScheduler invalidationScheduler, TimeSpan invalidationInterval,
            IEqualityComparer<K> keyComparer) : base(keyComparer) {
            _clock = clock;
            _invalidation = new SingleAssignmentDisposable {
                Disposable = Observable.Interval(invalidationInterval, invalidationScheduler)
                    .SelectMany(_ => Cache.Where(kvp => kvp.Value.Invalidation(kvp.Value)))
                    .SelectMany(UpdateEntry)
                    .Subscribe(kvp => Cache.AddOrUpdate(kvp.Key, _ => kvp.Value, (_1, _2) => kvp.Value))
            };
        }

        private IObservable<KeyValuePair<K, UpdateableCacheEntry<V>>> UpdateEntry(
            KeyValuePair<K, UpdateableCacheEntry<V>> kvp) {
            return kvp.Value.RunUpdate(_clock)
                .Select(e => kvp.MapV(_ => e));
        }

        public override void AddOrReplace(K key, V value) {
            AddOrReplace(key, value, _ => false, (oldValue, cancel) => {
                var tcs = new TaskCompletionSource<V>();
                tcs.SetResult(value);
                return tcs.Task;
            });
        }

        public void Dispose() {
            _invalidation.Dispose();
        }

        public void AddOrReplace(K key, V value, Func<UpdateableCacheEntry<V>, bool> invalidation,
            Func<V, CancellationToken, Task<V>> update) {
            var cacheEntry = UpdateableCacheEntry.From(value, DateTimeOffset.UtcNow, invalidation, update);

            Cache.AddOrUpdate(key, cacheEntry, (k, e) => UpdateableCacheEntry.From(e, value, _clock.UtcNow));
        }
    }
}