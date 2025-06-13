using Palettes.Api.StampPaletteApi;

namespace Palettes.Api
{
    public interface IApiClient :
        IAsyncDisposable,
        IDisposable,
        IStampPaletteApi;
}
