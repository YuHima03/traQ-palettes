using System.Text.Json.Serialization;

namespace Palettes.Api.StampPaletteApi
{
    public sealed class GetStampPaletteResult
    {
        [JsonPropertyName("id")]
        public required Guid Id { get; init; }

        [JsonPropertyName("creator")]
        public required User Creator { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("description")]
        public required string Description { get; init; }

        [JsonPropertyName("stamps")]
        public required Stamp[] Stamps { get; init; }

        [JsonPropertyName("subscribers")]
        public required Subscription[] Subscriptions { get; init; }

        [JsonPropertyName("createdAt")]
        public required DateTimeOffset CreatedAt { get; init; }

        [JsonPropertyName("updatedAt")]
        public required DateTimeOffset UpdatedAt { get; init; }

        public readonly struct Stamp
        {
            [JsonPropertyName("id")]
            public required Guid Id { get; init; }

            [JsonPropertyName("name")]
            public required string Name { get; init; }
        }

        public readonly struct Subscription
        {
            [JsonPropertyName("user")]
            public required User User { get; init; }

            [JsonPropertyName("createdAt")]
            public required DateTimeOffset CreatedAt { get; init; }
        }

        public readonly struct User
        {
            [JsonPropertyName("id")]
            public required Guid Id { get; init; }

            [JsonPropertyName("name")]
            public required string Name { get; init; }

            [JsonPropertyName("displayName")]
            public required string DisplayName { get; init; }
        }
    }
}
