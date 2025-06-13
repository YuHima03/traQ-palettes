using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Palettes.Api;

namespace Palettes.App.Client.Components.Pages
{
    public class AuthenticatedPageBase : ComponentBase
    {
        [Inject]
        public required IApiClientFactory ApiClientFactory { get; set; }

        [Inject]
        public required AuthenticationStateProvider AuthStateProvider { get; set; }

        public AuthenticatedUser? User { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

            await FetchAndSetUserAsync();
        }

        async void OnAuthenticationStateChanged(Task<AuthenticationState> authStateTask)
        {
            await FetchAndSetUserAsync();
            StateHasChanged();
        }

        async ValueTask FetchAndSetUserAsync()
        {
            await using var api = await ApiClientFactory.CreateApiClientAsync();
            var getMeResult = await api.GetMeAsync();
            if (getMeResult.IsSuccessStatusCode && getMeResult.Result is not null)
            {
                var u = getMeResult.Result;
                User = new() { Id = u.Id, Name = u.Name };
            }
            else
            {
                User = null;
            }
        }
    }

    public sealed class AuthenticatedUser
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }
    }
}
