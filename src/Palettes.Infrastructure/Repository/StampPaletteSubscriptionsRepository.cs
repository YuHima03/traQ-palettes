using Palettes.Domain.Models;
using Palettes.Domain.Repository;

namespace Palettes.Infrastructure.Repository
{
    public sealed partial class Repository : IStampPaletteSubscriptionsRepository
    {
        public ValueTask DeleteStampPaletteSubscriptionAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public ValueTask<StampPaletteSubscription> GetStampPaletteSubscriptionAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public ValueTask<StampPaletteSubscription[]> GetUserStampPaletteSubscriptionsAsync(Guid userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public ValueTask<StampPaletteSubscription> PostStampPaletteSubscriptionAsync(PostStampPaletteSubscriptionRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public ValueTask<StampPaletteSubscription> UpdateStampPaletteSubscriptionAsync(Guid id, UpdateStampPaletteSubscriptionRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
