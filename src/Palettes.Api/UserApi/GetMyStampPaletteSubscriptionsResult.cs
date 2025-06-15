using System.Text.Json.Serialization;

namespace Palettes.Api.UserApi
{
    public sealed class GetMyStampPaletteSubscriptionsResult
    {
        [JsonPropertyName("subscriptions")]
        public required Subscription[] Subscriptions { get; init; }

        public sealed class Subscription
        {
            [JsonPropertyName("id")]
            public required Guid Id { get; init; }

            [JsonPropertyName("stampPalette")]
            public required StampPaletteApi.GetStampPaletteListResult.StampPaletteAbstraction StampPalette { get; init; }

            [JsonPropertyName("copiedStampPaletteId")]
            public required Guid CopiedStampPaletteId { get; init; }

            [JsonPropertyName("createdAt")]
            public required DateTimeOffset CreatedAt { get; init; }

            [JsonPropertyName("syncedAt")]
            public required DateTimeOffset SyncedAt { get; init; }
        }
    }
}
