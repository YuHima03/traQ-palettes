using System.Text.Json.Serialization;

namespace Palettes.Api.UserApi
{
    public sealed class GetMeResult
    {
        [JsonPropertyName("id")]
        public required Guid Id { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }
    }
}
