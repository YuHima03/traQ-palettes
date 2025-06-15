using Palettes.Api;
using Palettes.Api.StampPaletteApi;
using Palettes.Api.UserApi;

namespace Palettes.App.Client.ApiClient
{
    sealed partial class ApiClient : IUserApi
    {
        public ValueTask<ApiResult<GetMeResult>> GetMeAsync(CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetMeResult>(HttpClient, "users/me", ct);
        }

        public ValueTask<ApiResult<GetStampPaletteListResult>> GetMyStampPalettesAsync(CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetStampPaletteListResult>(HttpClient, "users/me/stamp-palettes", ct);
        }

        public ValueTask<ApiResult<GetMyStampPaletteSubscriptionsResult>> GetMyStampPaletteSubscriptionsAsync(CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetMyStampPaletteSubscriptionsResult>(HttpClient, "users/me/stamp-palette-subscriptions", ct);
        }
    }
}
