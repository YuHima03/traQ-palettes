using Microsoft.EntityFrameworkCore;
using Palettes.Domain.Models;
using Palettes.Domain.Repository;
using Palettes.Infrastructure.Repository.Models;

namespace Palettes.Infrastructure.Repository
{
    public sealed partial class Repository : IStampPaletteSubscriptionsRepository
    {
        public async ValueTask DeleteStampPaletteSubscriptionAsync(Guid id, CancellationToken ct)
        {
            StampPaletteSubscriptions.Remove(await StampPaletteSubscriptions
                .Where(s => s.Id == id)
                .SingleAsync(ct));
            await SaveChangesAsync(ct);
        }

        public async ValueTask<StampPaletteSubscription> GetStampPaletteSubscriptionAsync(Guid id, CancellationToken ct)
        {
            return await StampPaletteSubscriptions
                .Where(s => s.Id == id)
                .Select(v => v.ToStampPaletteSubscription())
                .SingleAsync(ct);
        }

        public async ValueTask<StampPaletteSubscription[]> GetUserStampPaletteSubscriptionsAsync(Guid userId, CancellationToken ct)
        {
            return await StampPaletteSubscriptions
                .Where(s => s.UserId == userId)
                .Select(v => v.ToStampPaletteSubscription())
                .ToArrayAsync(ct);
        }

        public async ValueTask<StampPaletteSubscription> PostStampPaletteSubscriptionAsync(PostStampPaletteSubscriptionRequest request, CancellationToken ct)
        {
            RepoStampPaletteSubscription repoModel = new()
            {
                Id = Guid.CreateVersion7(),
                UserId = request.UserId,
                PaletteId = request.StampPaletteId,
                CopiedPaletteId = Guid.Empty,
                SyncedAt = request.SyncedAt.UtcDateTime,
            };
            StampPaletteSubscriptions.Add(repoModel);
            await SaveChangesAsync(ct);
            return repoModel.ToStampPaletteSubscription();
        }

        public async ValueTask<StampPaletteSubscription?> TryGetStampPaletteSubscriptionAsync(Guid userId, Guid paletteId, CancellationToken ct)
        {
            var subscription = await StampPaletteSubscriptions
                .Where(s => s.UserId == userId && s.PaletteId == paletteId)
                .Select(v => v.ToStampPaletteSubscription())
                .FirstOrDefaultAsync(ct);
            return subscription;
        }

        public async ValueTask<StampPaletteSubscription> UpdateStampPaletteSubscriptionAsync(Guid id, UpdateStampPaletteSubscriptionRequest request, CancellationToken ct)
        {
            var subscription = await StampPaletteSubscriptions
                .Where(s => s.Id == id)
                .SingleAsync(ct);
            subscription.SyncedAt = request.SyncedAt.UtcDateTime;
            await SaveChangesAsync(ct);
            return subscription.ToStampPaletteSubscription();
        }
    }

    static class StampPaletteSubscriptionsRepositoryExtension
    {
        public static StampPaletteSubscription ToStampPaletteSubscription(this RepoStampPaletteSubscription repositoryModel)
        {
            return new StampPaletteSubscription(
                repositoryModel.Id,
                repositoryModel.UserId,
                repositoryModel.PaletteId,
                repositoryModel.CopiedPaletteId,
                repositoryModel.SyncedAt,
                repositoryModel.CreatedAt,
                repositoryModel.UpdatedAt
                );
        }
    }
}
