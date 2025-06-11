using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Palettes.App.Services;
using Palettes.Utils.Authentication;
using Palettes.Utils.Authentication.Claims;

namespace Palettes.App.Components.Pages
{
    public class AuthenticatedPageBase : ComponentBase
    {
#pragma warning disable CS8618
        [Inject]
        AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Inject]
        AuthenticatedTraqHttpClientFactory TraqHttpClientFactory { get; set; }

        [Inject]
        protected Domain.Repository.IRepositoryFactory RepositoryFactory { get; set; }
#pragma warning restore CS8618

        protected override async Task OnInitializedAsync()
        {
            var client = await CreateTraqAuthenticatedHttpClientAsync().ConfigureAwait(false);
            if (client is null || !await IsLoggedInAsync(new Traq.Api.MeApi(client)).ConfigureAwait(false))
            {
                // Not logged in or traQ session is expired.
                var redirect = $"/{Navigation.ToBaseRelativePath(Navigation.Uri).TrimStart('/')}";
                Navigation.NavigateTo($"logout?redirect={Uri.EscapeDataString(redirect)}", true);
                return;
            }
            await base.OnInitializedAsync().ConfigureAwait(false);
        }

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

        protected async ValueTask<AuthenticatedUserInfo?> TryGetTraqUserInfoAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);
            if (authState.User.Identity is not { IsAuthenticated: true })
            {
                return null;
            }
            try
            {
                return authState.User.ToTraqUserInfo();
            }
            catch
            {
                return null;
            }
        }

        static async ValueTask<bool> IsLoggedInAsync(Traq.Api.MeApi meApi)
        {
            try
            {
                var response = await meApi.GetMeWithHttpInfoAsync().ConfigureAwait(false);
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Traq.Client.ApiException e) when (e.ErrorCode == StatusCodes.Status401Unauthorized)
            {
                return false;
            }
        }
    }
}
