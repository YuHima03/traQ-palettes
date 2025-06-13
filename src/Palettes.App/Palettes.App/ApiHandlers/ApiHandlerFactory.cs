using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Palettes.Api;
using Palettes.Domain.Repository;
using Palettes.Utils.Authentication.Claims;

namespace Palettes.App.ApiHandlers
{
    sealed class ApiHandlerFactory(
        IMemoryCache cache,
        ILogger<ApiHandler> handlerLogger,
        IHttpContextAccessor httpContextAccessor,
        IRepositoryFactory repositoryFactory,
        IOptions<Configurations.TraqClientOptions> traqOptions
        )
        : IApiClientFactory
    {
        public ValueTask<IApiClient> CreateApiClientAsync(CancellationToken ct = default)
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext is not available. Ensure that the factory is used within a valid HTTP request context.");

            if (httpContext.User.Identity is not { IsAuthenticated: true })
            {
                throw new UnauthorizedAccessException("User is not authenticated. Ensure that the user is logged in before creating an API client.");
            }

            var userInfo = httpContext.User.ToTraqUserInfo();
            ApiHandler handler = new(
                cache,
                handlerLogger,
                repositoryFactory,
                new Traq.TraqApiClient(Options.Create(new Traq.TraqApiClientOptions
                {
                    BaseAddress = traqOptions.Value.ApiBaseAddress!,
                    BearerAuthToken = userInfo.AccessToken
                })
            ));

            return ValueTask.FromResult(handler as IApiClient);
        }
    }
}
