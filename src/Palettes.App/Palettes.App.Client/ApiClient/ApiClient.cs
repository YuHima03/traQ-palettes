using Palettes.Api;
using System.Net.Http.Json;

namespace Palettes.App.Client.ApiClient
{
    sealed partial class ApiClient(
        HttpClient httpClient,
        ILogger<ApiClient> logger
        )
        : IApiClient
    {
        readonly HttpClient HttpClient = httpClient;
        readonly ILogger<ApiClient> Logger = logger;

        public void Dispose() { }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        static async ValueTask<ApiResult<T>> GetApiResultFromJsonAsync<T>(HttpClient client, string? requestUri, CancellationToken ct = default)
        {
            var response = await client.GetAsync(requestUri, ct);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>(ct);
                return ApiResult.Ok(result ?? default!);
            }
            else
            {
                return new ApiResult<T> { Result = default!, StatusCode = response.StatusCode };
            }
        }

        static async ValueTask<ApiResult<T>> GetApiResultFromJsonAsync<T>(HttpResponseMessage response, CancellationToken ct = default)
        {
            if (response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return ApiResult.Ok(await response.Content.ReadFromJsonAsync<T>(ct) ?? default!);

                    case System.Net.HttpStatusCode.Created:
                        return ApiResult.Created(await response.Content.ReadFromJsonAsync<T>(ct) ?? default!);
                }
            }
            return new ApiResult<T> { Result = default!, StatusCode = response.StatusCode };
        }
    }
}
