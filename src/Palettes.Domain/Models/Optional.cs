namespace Palettes.Domain.Models
{
    public readonly struct Optional<T>()
    {
        public T Value { get; private init; } = default!;

        public bool HasValue { get; private init; } = false;

        public static Optional<T> None => new() { Value = default!, HasValue = false };

        internal static Optional<T> Create(T value) => new() { Value = value, HasValue = true };
    }

    public static class Optional
    {
        public static Optional<T> Create<T>(T value) => Optional<T>.Create(value);

        public static Optional<T?> CreateNotNull<T>(T value) where T : struct
            => Optional<T?>.Create(new T?(value));
    }
}
