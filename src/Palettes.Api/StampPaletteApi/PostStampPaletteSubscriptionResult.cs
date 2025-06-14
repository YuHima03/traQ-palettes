using System.Text.Json.Serialization;

namespace Palettes.Api.StampPaletteApi
{
    public sealed class PostStampPaletteSubscriptionResult
    {
        [JsonPropertyName("subscriptionId")]
        public Guid? SubscriptionId { get; init; }
    }
}
