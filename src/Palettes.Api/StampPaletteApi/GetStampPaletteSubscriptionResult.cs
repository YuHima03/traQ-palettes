using System.Text.Json.Serialization;

namespace Palettes.Api.StampPaletteApi
{
    public sealed class GetStampPaletteSubscriptionResult
    {
        [JsonPropertyName("id")]
        public required Guid Id { get; init; }

        [JsonPropertyName("copiedStampPaletteId")]
        public required Guid CopiedStampPaletteId { get; init; }

        [JsonPropertyName("createdAt")]
        public required DateTimeOffset CreatedAt { get; init; }

        [JsonPropertyName("syncedAt")]
        public required DateTimeOffset SyncedAt { get; init; }
    }
}
