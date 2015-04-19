using System;
using System.Collections.Generic;

namespace NCaching.Extensions {
    internal static class KeyValuePairExt {
        public static KeyValuePair<K, V> MapV<K, V>(this KeyValuePair<K, V> kvp, Func<V, V> f) {
            var newValue = f(kvp.Value);
            return new KeyValuePair<K, V>(kvp.Key, newValue);
        }
    }
}