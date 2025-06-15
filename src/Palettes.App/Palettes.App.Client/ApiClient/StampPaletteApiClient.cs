using Palettes.Api;
using Palettes.Api.StampPaletteApi;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Palettes.App.Client.ApiClient
{
    sealed partial class ApiClient : IStampPaletteApi
    {
        public async ValueTask<ApiResult> DeleteStampPaletteSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            var res = await HttpClient.DeleteAsync($"stamp-palettes/{stampPaletteId}/subscription", ct);
            return await GetApiResultFromJsonAsync<object>(res, ct);
        }

        public ValueTask<ApiResult<GetStampPaletteListResult>> GetPublicStampPalettesAsync(CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetStampPaletteListResult>(HttpClient, "stamp-palettes", ct);
        }

        public ValueTask<ApiResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id, CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetStampPaletteResult>(HttpClient, $"stamp-palettes/{id}", ct);
        }

        public ValueTask<ApiResult<GetStampPaletteSubscriptionResult>> GetStampPaletteSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetStampPaletteSubscriptionResult>(HttpClient, $"stamp-palettes/{stampPaletteId}/subscription", ct);
        }

        public async ValueTask<ApiResult> PatchStampPaletteAsync(Guid id, PatchStampPaletteRequest request, CancellationToken ct = default)
        {
            var res = await HttpClient.PatchAsync($"stamp-palettes/{id}", JsonContent.Create(request, MediaTypeHeaderValue.Parse("application/json")), ct);
            return await GetApiResultFromJsonAsync<object>(res, ct);
        }

        public async ValueTask<ApiResult<PostStampPaletteSubscriptionResult>> PostStampPalletSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            var res = await HttpClient.PostAsync($"stamp-palettes/{stampPaletteId}/subscription", null, ct);
            return await GetApiResultFromJsonAsync<PostStampPaletteSubscriptionResult>(res, ct);
        }

        public async ValueTask<ApiResult<GetStampPaletteSubscriptionResult>> SyncCloneStampPaletteAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            var res = await HttpClient.PostAsync($"stamp-palettes/{stampPaletteId}/sync", null, ct);
            return await GetApiResultFromJsonAsync<GetStampPaletteSubscriptionResult>(res, ct);
        }
    }
}
