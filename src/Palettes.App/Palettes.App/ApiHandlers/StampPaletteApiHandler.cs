using Palettes.Api;
using Palettes.Api.StampPaletteApi;
using Palettes.Utils.Caching.Traq;

namespace Palettes.App.ApiHandlers
{
    sealed partial class ApiHandler : IStampPaletteApi
    {
        public async ValueTask<ApiResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id, CancellationToken ct = default)
        {
            var stampPaletteTask = Task.Run(async () => 
            {
                await using var repo = await RepositoryFactory.CreateRepositoryAsync(ct);
                return await repo.TryGetStampPaletteAsync(id, ct);
            });
            var traqStampPaletteTask = TraqClient.StampApi.GetCachedStampPaletteAsync(Cache, id, ct).AsTask();
            var traqUsersTask = TraqClient.UserApi.GetCachedUsersAsync(Cache, true, ct).AsTask();
            var traqStampsTask = TraqClient.StampApi.GetCachedStampsAsync(Cache, null, ct).AsTask();

            await Task.WhenAll(stampPaletteTask, traqStampPaletteTask, traqUsersTask, traqStampsTask);

            var stampPalette = stampPaletteTask.Result;
            var traqStampPalette = traqStampPaletteTask.Result;
            var traqUsers = traqUsersTask.Result;
            var traqStamps = traqStampsTask.Result;

            if (traqStampPalette is null
                || !traqUsers.TryGetValue(traqStampPalette.CreatorId, out var creator))
            {
                return ApiResult.NotFound<GetStampPaletteResult>();
            }

            return ApiResult.Ok(new GetStampPaletteResult
            {
                Id = traqStampPalette.Id,
                Creator = new GetStampPaletteResult.User
                {
                    Id = creator.Id,
                    Name = creator.Name,
                    DisplayName = creator.DisplayName
                },
                Name = traqStampPalette.Name,
                Description = traqStampPalette.Description,
                Stamps = [.. traqStampPalette.Stamps
                    .Select(stampId => new GetStampPaletteResult.Stamp
                    {
                        Id = stampId,
                        Name = traqStamps.TryGetValue(stampId, out var s) ? s.Name : ""
                    })],
                Subscriptions = stampPalette?.Subscribers.Select(s =>
                {
                    var u = traqUsers.GetValueOrDefault(s.UserId);
                    return new GetStampPaletteResult.Subscription
                    {
                        User = new GetStampPaletteResult.User
                        {
                            Id = s.UserId,
                            Name = u?.Name!,
                            DisplayName = u?.DisplayName!
                        },
                        CreatedAt = s.CreatedAt
                    };
                }).ToArray() ?? [],
                IsPublic = stampPalette?.IsPublic ?? false,
                CreatedAt = traqStampPalette.CreatedAt,
                UpdatedAt = traqStampPalette.UpdatedAt
            });
        }
    }
}
