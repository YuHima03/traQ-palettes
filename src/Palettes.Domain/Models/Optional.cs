using System.Text.Json;
using System.Text.Json.Serialization;

namespace Palettes.Domain.Models
{
    [JsonConverter(typeof(OptionalJsonConverterFactory))]
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

    file sealed class OptionalJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var genericType = typeToConvert.GetGenericArguments()[0];
            var converterType = typeof(OptionalJsonConverter<>).MakeGenericType(genericType);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }
    }

    file sealed class OptionalJsonConverter<T> : JsonConverter<Optional<T>>
    {
        public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = JsonSerializer.Deserialize<T>(ref reader, options);
            if (typeof(T).IsValueType && value is null)
            {
                return Optional<T>.None;
            }
            return Optional<T>.Create(value!);
        }

        public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                JsonSerializer.Serialize(writer, value.Value, options);
            }
        }
    }
}
