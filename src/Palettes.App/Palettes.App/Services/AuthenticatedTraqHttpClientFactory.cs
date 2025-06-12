using Microsoft.AspNetCore.Components.Authorization;

namespace Palettes.App.Services
{
    sealed class AuthenticatedTraqHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        public HttpClient CreateClient(AuthenticationStateProvider authStateProvider)
        {
            return CreateClientAsync(authStateProvider).AsTask().GetAwaiter().GetResult();
        }

        public async ValueTask<HttpClient> CreateClientAsync(AuthenticationStateProvider authStateProvider)
        {
            var client = httpClientFactory.CreateClient(Configurations.TraqClientConfigurator.AuthenticatedHttpClientName);
            var authState = await authStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
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
