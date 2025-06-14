using Palettes.Domain.Models;
using System.Text.Json.Serialization;

namespace Palettes.Api.StampPaletteApi
{
    public sealed class PatchStampPaletteRequest
    {
        [JsonPropertyName("isPublic")]
        public Optional<bool> IsPublic { get; init; }
    }
}
