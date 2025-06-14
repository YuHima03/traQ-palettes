using Palettes.Api;
using Palettes.Api.StampPaletteApi;
using Palettes.Utils.Caching.Traq;

namespace Palettes.App.ApiHandlers
{
    sealed partial class ApiHandler : IStampPaletteApi
    {
        public async ValueTask<ApiResult> DeleteStampPaletteSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            if (AuthenticatedUser is null)
            {
                return ApiResult.Unauthorized<object>();
            }
            await using var repo = await RepositoryFactory.CreateRepositoryAsync(ct);
            var sub = await repo.TryGetStampPaletteSubscriptionAsync(AuthenticatedUser.Id, stampPaletteId, ct);
            if (sub is null)
            {
                return ApiResult.NotFound<object>();
            }
            await repo.DeleteStampPaletteSubscriptionAsync(sub.Id, ct);
            return ApiResult.NoContent<object>();
        }

        public async ValueTask<ApiResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id, CancellationToken ct = default)
        {
            if (AuthenticatedUser is null)
            {
                return ApiResult.Unauthorized<GetStampPaletteResult>();
            }

            var stampPaletteTask = Task.Run(async () => {
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
            else if (stampPalette is not { IsPublic: true } && traqStampPalette.CreatorId != AuthenticatedUser.Id)
            {
                return ApiResult.Forbidden<GetStampPaletteResult>();
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
                Subscriptions = stampPalette?.Subscribers.Select(s => {
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

        public async ValueTask<ApiResult> PatchStampPaletteAsync(Guid id, PatchStampPaletteRequest request, CancellationToken ct = default)
        {
            if (!request.IsPublic.HasValue)
            {
                return ApiResult.BadRequest<GetStampPaletteResult>("No properties are set.");
            }
            else if (AuthenticatedUser is null)
            {
                return ApiResult.Unauthorized<GetStampPaletteResult>();
            }
            await using var repo = await RepositoryFactory.CreateRepositoryAsync(ct);
            var stampPalette = await repo.TryGetStampPaletteAsync(id, ct);
            if (stampPalette is null)
            {
                try
                {
                    var traqStampPalette = await TraqClient.StampApi.GetCachedStampPaletteAsync(Cache, id, ct);
                    await repo.PostStampPaletteAsync(new Domain.Models.PostStampPaletteRequest(
                        traqStampPalette.Id,
                        traqStampPalette.CreatorId,
                        request.IsPublic.HasValue && request.IsPublic.Value
                        ),
                        ct
                    );
                    return ApiResult.NoContent<GetStampPaletteResult>();
                }
                catch (Traq.Client.ApiException e) when (e.ErrorCode == StatusCodes.Status404NotFound)
                {
                    return ApiResult.NotFound<GetStampPaletteResult>();
                }
            }
            else if (stampPalette.UserId != AuthenticatedUser.Id)
            {
                return ApiResult.Forbidden<GetStampPaletteResult>();
            }
            await repo.UpdateStampPaletteAsync(id, new Domain.Models.UpdateStampPaletteRequest { IsPublic = request.IsPublic.Value }, ct);
            return ApiResult.NoContent<GetStampPaletteResult>();
        }

        public async ValueTask<ApiResult<PostStampPaletteSubscriptionResult>> PostStampPalletSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            if (AuthenticatedUser is null)
            {
                return ApiResult.Unauthorized<PostStampPaletteSubscriptionResult>();
            }
            await using var repo = await RepositoryFactory.CreateRepositoryAsync(ct);
            if (await repo.TryGetStampPaletteSubscriptionAsync(AuthenticatedUser!.Id, stampPaletteId, ct) is not null)
            {
                return ApiResult.ErrorStatusCode<PostStampPaletteSubscriptionResult>(System.Net.HttpStatusCode.Conflict);
            }
            var stampPalette = await repo.TryGetStampPaletteAsync(stampPaletteId, ct);
            if (stampPalette is null)
            {
                return ApiResult.NotFound<PostStampPaletteSubscriptionResult>();
            }
            else if (stampPalette.UserId == AuthenticatedUser.Id)
            {
                return ApiResult.BadRequest<PostStampPaletteSubscriptionResult>("You cannot subscribe to your own stamp palette.");
            }
            else if (!stampPalette.IsPublic)
            {
                return ApiResult.NotFound<PostStampPaletteSubscriptionResult>();
            }
            var sub = await repo.PostStampPaletteSubscriptionAsync(new Domain.Models.PostStampPaletteSubscriptionRequest
            {
                StampPaletteId = stampPaletteId,
                UserId = AuthenticatedUser.Id,
                SyncedAt = DateTimeOffset.MinValue
            }, ct);
            return ApiResult.Created(new PostStampPaletteSubscriptionResult
            {
                SubscriptionId = sub.Id
            });
        }
    }
}
