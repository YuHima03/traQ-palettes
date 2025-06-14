using Microsoft.Extensions.Caching.Memory;
using Palettes.App.Client.Components.Pages;

namespace Palettes.App.ApiHandlers
{
    sealed partial class ApiHandler(
        AuthenticatedUser? authenticatedUser,
        IMemoryCache cache,
        ILogger<ApiHandler> logger,
        Domain.Repository.IRepositoryFactory repositoryFactory,
        Traq.ITraqApiClient traqClient
        )
        : Api.IApiClient
    {
        readonly AuthenticatedUser? AuthenticatedUser = authenticatedUser;
        readonly IMemoryCache Cache = cache;
        readonly ILogger<ApiHandler> Logger = logger;
        readonly Domain.Repository.IRepositoryFactory RepositoryFactory = repositoryFactory;
        readonly Traq.ITraqApiClient TraqClient = traqClient;

        public void Dispose() { }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
