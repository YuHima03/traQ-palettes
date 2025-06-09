using Microsoft.AspNetCore.Components.Authorization;

namespace Palettes.App.Services
{
    sealed class AuthenticatedTraqHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        public async ValueTask<HttpClient> CreateClientAsync(AuthenticationStateProvider authStateProvider)
        {
            var client = httpClientFactory.CreateClient(Configurations.TraqClientConfigurator.AuthenticatedHttpClientName);
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity is not { IsAuthenticated: true })
            {
                return client;
            }
            var token = authState.User.FindFirst(Utils.Authentication.Claims.ClaimTypesInternal.TraqAccessToken);
            if (token is not null)
            {
                client.DefaultRequestHeaders.Authorization = new("Bearer", token.Value);
            }
            return client;
        }
    }
}
