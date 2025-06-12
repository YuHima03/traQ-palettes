namespace Palettes.Api.StampPaletteApi
{
    public interface IStampPaletteApi
    {
        public ValueTask<ApiResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id, CancellationToken ct = default);
    }
}
