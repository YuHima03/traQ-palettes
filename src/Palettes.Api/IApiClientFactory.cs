namespace Palettes.Api
{
    public interface IApiClientFactory
    {
        public IApiClient CreateApiClient()
        {
            return CreateApiClientAsync(default).AsTask().GetAwaiter().GetResult();
        }

        public ValueTask<IApiClient> CreateApiClientAsync(CancellationToken ct = default);
    }
}
