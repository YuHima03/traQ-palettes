using Palettes.Api;
using Palettes.Api.StampPaletteApi;

namespace Palettes.App.Client.ApiClient
{
    sealed partial class ApiClient : IStampPaletteApi
    {
        public async ValueTask<ApiResult> DeleteStampPaletteSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            var res = await HttpClient.DeleteAsync($"stamp-palettes/{stampPaletteId}/subscription", ct);
            return await GetApiResultFromJsonAsync<object>(res, ct);
        }

        public ValueTask<ApiResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id, CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetStampPaletteResult>(HttpClient, $"stamp-palettes/{id}", ct);
        }

        public async ValueTask<ApiResult<PostStampPaletteSubscriptionResult>> PostStampPalletSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            var res = await HttpClient.PostAsync($"stamp-palettes/{stampPaletteId}/subscription", null, ct);
            return await GetApiResultFromJsonAsync<PostStampPaletteSubscriptionResult>(res, ct);
        }
    }
}
