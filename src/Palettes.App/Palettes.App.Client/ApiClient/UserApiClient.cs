using Palettes.Api;
using Palettes.Api.UserApi;

namespace Palettes.App.Client.ApiClient
{
    sealed partial class ApiClient : IUserApi
    {
        public ValueTask<ApiResult<GetMeResult>> GetMeAsync(CancellationToken ct = default)
        {
            return GetApiResultFromJsonAsync<GetMeResult>(HttpClient, "users/me", ct);
        }
    }
}
