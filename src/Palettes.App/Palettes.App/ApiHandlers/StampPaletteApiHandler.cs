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

        public async ValueTask<ApiResult<GetStampPaletteListResult>> GetPublicStampPalettesAsync(CancellationToken ct = default)
        {
            if (AuthenticatedUser is null)
            {
                return ApiResult.Unauthorized<GetStampPaletteListResult>();
            }
            var stampPalettesTask = Task.Run(async () =>
            {
                await using var repo = await RepositoryFactory.CreateRepositoryAsync(ct);
                return await (await repo.GetPublicStampPalettesAsync(ct)).ToAsyncEnumerable()
                    .SelectAwait(async p =>
                    {
                        try
                        {
                            return ValueTuple.Create<Domain.Models.StampPalette, Traq.Model.StampPalette?>(p, await TraqClient.StampApi.GetCachedStampPaletteAsync(Cache, p.Id, ct));
                        }
                        catch (Traq.Client.ApiException e) when (e.ErrorCode == StatusCodes.Status404NotFound)
                        {
                            return (p, null);
                        }
                    })
                    .ToListAsync(ct);
            });
            var traqStampsTask = TraqClient.StampApi.GetCachedStampsAsync(Cache, null, ct).AsTask();
            var traqUsersTask = TraqClient.UserApi.GetCachedUsersAsync(Cache, true, ct).AsTask();

            await Task.WhenAll(stampPalettesTask, traqUsersTask);

            var stampPalettes = stampPalettesTask.Result;
            var traqStamps = traqStampsTask.Result;
            var traqUsers = traqUsersTask.Result;

            return ApiResult.Ok(new GetStampPaletteListResult
            {
                StampPalettes = [.. stampPalettes
                    .Select(p =>
                    {
                        var (sp, qsp) = p;
                        if (qsp is null)
                        {
                            return null!;
                        }
                        var creator = traqUsers.GetValueOrDefault(sp.UserId);
                        if (creator is null)
                        {
                            return null!;
                        }
                        return new GetStampPaletteListResult.StampPaletteAbstraction
                        {
                            Id = sp.Id,
                            Name = qsp.Name,
                            Creator = new GetStampPaletteResult.User
                            {
                                Id = creator.Id,
                                Name = creator.Name ,
                                DisplayName = creator.DisplayName
                            },
                            IsPublic = sp.IsPublic,
                            StampSamples = [.. qsp.Stamps
                                .Take(20)
                                .Select(stampId => new GetStampPaletteResult.Stamp
                                {
                                    Id = stampId,
                                    Name = traqStamps.TryGetValue(stampId, out var s) ? s.Name : ""
                                })],
                            StampCount = qsp.Stamps.Count,
                            SubscriberCount = sp.Subscribers.Length,
                            CreatedAt = qsp.CreatedAt,
                            UpdatedAt = qsp.UpdatedAt
                        };
                    })
                    .Where(p => p is not null)] // exclude if stamp palette or user is not found on traQ.
            });
        }

        public async ValueTask<ApiResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id, CancellationToken ct = default)
        {
            if (AuthenticatedUser is null)
            {
                return ApiResult.Unauthorized<GetStampPaletteResult>();
            }

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

        public async ValueTask<ApiResult<GetStampPaletteSubscriptionResult>> GetStampPaletteSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default)
        {
            if (AuthenticatedUser is null)
            {
                return ApiResult.Unauthorized<GetStampPaletteSubscriptionResult>();
            }
            await using var repo = await RepositoryFactory.CreateRepositoryAsync(ct);
            var stampPalette = await repo.TryGetStampPaletteAsync(stampPaletteId, ct);
            if (stampPalette?.UserId == AuthenticatedUser.Id)
            {
                return ApiResult.BadRequest<GetStampPaletteSubscriptionResult>("You are an owner of the stamp palette.");
            }
            else if (stampPalette is not { IsPublic: true })
            {
                return ApiResult.NotFound<GetStampPaletteSubscriptionResult>();
            }
            var subscription = await repo.TryGetStampPaletteSubscriptionAsync(AuthenticatedUser.Id, stampPaletteId, ct);
            if (subscription is null)
            {
                return ApiResult.NotFound<GetStampPaletteSubscriptionResult>();
            }
            return ApiResult.Ok(new GetStampPaletteSubscriptionResult
            {
                Id = subscription.Id,
                CopiedStampPaletteId = subscription.CopiedStampPaletteId,
                CreatedAt = subscription.CreatedAt,
                SyncedAt = subscription.SyncedAt
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
