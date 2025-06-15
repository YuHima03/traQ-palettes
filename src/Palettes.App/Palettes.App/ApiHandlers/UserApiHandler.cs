using Palettes.Api;
using Palettes.Api.StampPaletteApi;
using Palettes.Api.UserApi;
using Palettes.Utils.Caching.Traq;

namespace Palettes.App.ApiHandlers
{
    sealed partial class ApiHandler : IUserApi
    {
        public async ValueTask<ApiResult<GetMeResult>> GetMeAsync(CancellationToken ct = default)
        {
            try
            {
                var res = await TraqClient.MeApi.GetMeWithHttpInfoAsync(ct);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return ApiResult.Ok(new GetMeResult
                    {
                        Id = res.Data.Id,
                        Name = res.Data.Name
                    });
                }
                return ApiResult.Unauthorized<GetMeResult>();
            }
            catch (Traq.Client.ApiException e) when (e.ErrorCode == StatusCodes.Status401Unauthorized)
            {
                return ApiResult.Unauthorized<GetMeResult>();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while getting user info");
                return ApiResult.InternalServerError<GetMeResult>();
            }
        }

        public async ValueTask<ApiResult<GetStampPaletteListResult>> GetMyStampPalettesAsync(CancellationToken ct = default)
        {
            if (AuthenticatedUser is null)
            {
                return ApiResult.Unauthorized<GetStampPaletteListResult>();
            }

            var traqStampsTask = TraqClient.StampApi.GetCachedStampsAsync(Cache, null, ct).AsTask();
            var myTraqStampPalettesTask = TraqClient.StampApi.GetStampPalettesAsync(ct);
            await using var repo = await RepositoryFactory.CreateRepositoryAsync(ct);
            var myStampPalettes = (await repo.GetUserStampPalettesAsync(AuthenticatedUser.Id, true, ct)).ToDictionary(sp => sp.Id);
            var myStampPaletteSubscriptions= await repo.GetUserStampPaletteSubscriptionsAsync(AuthenticatedUser.Id, ct);

            await Task.WhenAll( myTraqStampPalettesTask, traqStampsTask);

            var myTraqStampPalettes = myTraqStampPalettesTask.Result;
            var traqStamps = traqStampsTask.Result;

            var copiedStampPaletteIds = myStampPaletteSubscriptions
                .Select(s => s.StampPaletteId)
                .Where(id => id != Guid.Empty)
                .ToHashSet();
            GetStampPaletteResult.User creatorConst = new()
            {
                Id = AuthenticatedUser.Id,
                Name = AuthenticatedUser.Name,
                DisplayName = ""
            };
            return ApiResult.Ok(new GetStampPaletteListResult
            {
                StampPalettes = [.. myTraqStampPalettes
                    .Where(sp => !copiedStampPaletteIds.Contains(sp.Id)) // exclude copied stamp palettes
                    .Select(qsp =>
                    {
                        var sp = myStampPalettes.GetValueOrDefault(qsp.Id);
                        return new GetStampPaletteListResult.StampPaletteAbstraction
                        {
                            Id = qsp.Id,
                            Name = qsp.Name,
                            Creator = creatorConst,
                            IsPublic = sp?.IsPublic ?? false,
                            StampSamples = [.. qsp.Stamps
                                .Take(20)
                                .Select(stampId => new GetStampPaletteResult.Stamp
                                {
                                    Id = stampId,
                                    Name = traqStamps.TryGetValue(stampId, out var s) ? s.Name : ""
                                })],
                            StampCount = qsp.Stamps.Count,
                            SubscriberCount = sp?.Subscribers.Length ?? 0,
                            CreatedAt = qsp.CreatedAt,
                            UpdatedAt = qsp.UpdatedAt
                        };
                    })]
            });
        }
    }
}
