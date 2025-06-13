using Microsoft.Extensions.Caching.Memory;

namespace Palettes.App.ApiHandlers
{
    sealed partial class ApiHandler(
        IMemoryCache cache,
        ILogger<ApiHandler> logger,
        Domain.Repository.IRepositoryFactory repositoryFactory,
        Traq.ITraqApiClient traqClient
        ) : Api.IApiClient
    {
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
