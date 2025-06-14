using Palettes.Api;

namespace Palettes.App.Client.ApiClient
{
    sealed class ApiClientFactory(
        HttpClient httpClient,
        ILoggerFactory loggers
        )
        : IApiClientFactory
    {
        readonly ApiClient _apiClient = new(httpClient, loggers.CreateLogger<ApiClient>());

        public ValueTask<IApiClient> CreateApiClientAsync(CancellationToken ct = default)
        {
            if (ct.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<IApiClient>(ct);
            }
            return ValueTask.FromResult(_apiClient as IApiClient);
        }
    }
}
