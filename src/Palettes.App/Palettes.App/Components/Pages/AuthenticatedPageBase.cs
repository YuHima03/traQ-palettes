using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Palettes.App.Services;

namespace Palettes.App.Components.Pages
{
    public class AuthenticatedPageBase : ComponentBase
    {
#pragma warning disable CS8618
        [Inject]
        AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        AuthenticatedTraqHttpClientFactory TraqHttpClientFactory { get; set; }

        [Inject]
        protected Domain.Repository.IRepositoryFactory RepositoryFactory { get; set; }
#pragma warning restore CS8618

        protected HttpClient? CreateTraqAuthenticatedHttpClient()
        {
            return CreateTraqAuthenticatedHttpClientAsync().AsTask().GetAwaiter().GetResult();
        }

        protected async ValueTask<HttpClient?> CreateTraqAuthenticatedHttpClientAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
            if (authState.User.Identity is not { IsAuthenticated: true })
            {
                return null;
            }
            return await TraqHttpClientFactory.CreateClientAsync(AuthStateProvider).ConfigureAwait(false);
        }
    }
}
