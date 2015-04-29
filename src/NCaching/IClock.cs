using System;

namespace NCaching {
    public interface IClock {
        DateTimeOffset UtcNow { get; }
    }

    public sealed class StandardClock : IClock {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}