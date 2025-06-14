using Palettes.Api;
using Palettes.Api.StampPaletteApi;

namespace Palettes.App.Client.ApiClient
{
    sealed partial class ApiClient : IStampPaletteApi
    {
        public ValueTask<ApiResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id, CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetStampPaletteResult>(HttpClient, $"stamp-palettes/{id}", ct);
        }
    }
}
