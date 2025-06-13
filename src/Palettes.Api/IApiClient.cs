using Palettes.Api.StampPaletteApi;
using Palettes.Api.UserApi;

namespace Palettes.Api
{
    public interface IApiClient :
        IAsyncDisposable,
        IDisposable,
        IStampPaletteApi,
        IUserApi;
}
