namespace Palettes.Api.UserApi
{
    public interface IUserApi
    {
        public ValueTask<ApiResult<GetMeResult>> GetMeAsync(CancellationToken ct = default);
    }
}
