using System;
using System.Collections.Generic;

namespace NCaching.Extensions {
    internal static class KeyValuePairExt {
        public static KeyValuePair<K, R> MapV<K, V, R>(this KeyValuePair<K, V> kvp, Func<V, R> f) {
            var newValue = f(kvp.Value);
            return new KeyValuePair<K, R>(kvp.Key, newValue);
        }
    }
}