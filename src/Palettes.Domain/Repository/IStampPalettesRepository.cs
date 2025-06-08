using Palettes.Domain.Models;

namespace Palettes.Domain.Repository
{
    public interface IStampPalettesRepository : IRepositoryBase
    {
        public ValueTask<StampPaletteRef> GetStampPaletteAsync(Guid id, CancellationToken ct);

        public ValueTask<StampPaletteRef[]> GetPublicStampPalettesAsync(CancellationToken ct);

        public ValueTask<StampPaletteRef[]> GetUserStampPalettesAsync(Guid userId, bool includePrivate, CancellationToken ct);

        public ValueTask<StampPaletteRef> PostStampPaletteAsync(PostStampPaletteRequest request, CancellationToken ct);

        public ValueTask<StampPaletteRef> UpdateStampPaletteAsync(Guid id, UpdateStampPaletteRequest request, CancellationToken ct);
    }
}
