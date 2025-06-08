namespace Palettes.App.Configurations
{
    public sealed class TraqClientOptions
    {
        [ConfigurationKeyName("TRAQ_API_BASE_ADDRESS")]
        public string? ApiBaseAddress { get; set; }

        [ConfigurationKeyName("TRAQ_CLIENT_ID")]
        public string? ClientId { get; set; }
    }
}
