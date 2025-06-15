using System.Text.Json.Serialization;

namespace Palettes.Api.StampPaletteApi
{
    public sealed class GetStampPaletteListResult
    {
        [JsonPropertyName("stampPalettes")]
        public required StampPaletteAbstraction[] StampPalettes { get; init; }

        public sealed class StampPaletteAbstraction
        {
            [JsonPropertyName("id")]
            public required Guid Id { get; init; }

            [JsonPropertyName("name")]
            public required string Name { get; init; }

            [JsonPropertyName("creator")]
            public required GetStampPaletteResult.User Creator { get; init; }

            [JsonPropertyName("isPublic")]
            public required bool IsPublic { get; init; }

            [JsonPropertyName("stampSamples")]
            public required GetStampPaletteResult.Stamp[] StampSamples { get; init; }

            [JsonPropertyName("stampCount")]
            public required int StampCount { get; init; }

            [JsonPropertyName("subscriberCount")]
            public required int SubscriberCount { get; init; }

            [JsonPropertyName("createdAt")]
            public required DateTimeOffset CreatedAt { get; init; }

            [JsonPropertyName("updatedAt")]
            public required DateTimeOffset UpdatedAt { get; init; }
        }
    }
}
