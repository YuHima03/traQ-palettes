using Palettes.Domain.Models;

namespace Palettes.Domain.Repository
{
    public interface IStampPalettesRepository : IRepositoryBase
    {
        public ValueTask<StampPalette> GetStampPaletteAsync(Guid id, CancellationToken ct);

        public ValueTask<StampPalette[]> GetPublicStampPalettesAsync(CancellationToken ct);

        public ValueTask<StampPalette[]> GetUserStampPalettesAsync(Guid userId, bool includePrivate, CancellationToken ct);

        public ValueTask<StampPalette> PostStampPaletteAsync(PostStampPaletteRequest request, CancellationToken ct);

        public ValueTask<StampPalette?> TryGetStampPaletteAsync(Guid id, CancellationToken ct);

        public ValueTask<StampPalette> UpdateStampPaletteAsync(Guid id, UpdateStampPaletteRequest request, CancellationToken ct);
    }
}
