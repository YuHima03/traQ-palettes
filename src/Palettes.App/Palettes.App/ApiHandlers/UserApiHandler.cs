using Palettes.Api;
using Palettes.Api.UserApi;

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
    }
}
