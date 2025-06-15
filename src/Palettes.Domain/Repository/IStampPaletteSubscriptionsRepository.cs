using Palettes.Domain.Models;

namespace Palettes.Domain.Repository
{
    public interface IStampPaletteSubscriptionsRepository : IRepositoryBase
    {
        public ValueTask DeleteStampPaletteSubscriptionAsync(Guid id, CancellationToken ct);

        public ValueTask<StampPaletteSubscription> GetStampPaletteSubscriptionAsync(Guid id, CancellationToken ct);

        public ValueTask<StampPaletteSubscription[]> GetUserStampPaletteSubscriptionsAsync(Guid userId, CancellationToken ct);

        public ValueTask<StampPaletteSubscriptionWithStampPalette[]> GetUserStampPaletteSubscriptionsWithStampPaletteAsync(Guid userId, CancellationToken ct);

        public ValueTask<StampPaletteSubscription> PostStampPaletteSubscriptionAsync(PostStampPaletteSubscriptionRequest request, CancellationToken ct);

        public ValueTask<StampPaletteSubscription?> TryGetStampPaletteSubscriptionAsync(Guid userId, Guid paletteId, CancellationToken ct);

        public ValueTask<StampPaletteSubscription> UpdateStampPaletteSubscriptionAsync(Guid id, UpdateStampPaletteSubscriptionRequest request, CancellationToken ct);
    }
}
