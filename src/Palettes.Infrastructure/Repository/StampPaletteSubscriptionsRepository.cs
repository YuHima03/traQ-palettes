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
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(v => v.ToStampPaletteSubscription())
                .SingleAsync(ct);
        }

        public async ValueTask<StampPaletteSubscription[]> GetUserStampPaletteSubscriptionsAsync(Guid userId, CancellationToken ct)
        {
            return await StampPaletteSubscriptions
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .Select(v => v.ToStampPaletteSubscription())
                .ToArrayAsync(ct);
        }

        public async ValueTask<StampPaletteSubscriptionWithStampPalette[]> GetUserStampPaletteSubscriptionsWithStampPaletteAsync(Guid userId, CancellationToken ct)
        {
            var subscriptions = await StampPaletteSubscriptions
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .ToListAsync(ct);
            var paletteIds = subscriptions.Select(s => s.PaletteId);
            var palettes = await StampPalettes
                .AsNoTracking()
                .Where(p => paletteIds.Contains(p.Id))
                .JoinSubscriptions(StampPaletteSubscriptions)
                .ToDictionaryAsync(p => p.Item1.Id, ct);
            return [.. subscriptions
                .Select(s => (s, palettes.GetValueOrDefault(s.PaletteId)))
                .Where(t => t.Item2 != default)
                .Select(t => t.ToStampPaletteSubscriptionWithStampPalette())
            ];
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
                .AsNoTracking()
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
            if (request.CopiedStampPaletteId is not null)
            {
                subscription.CopiedPaletteId = request.CopiedStampPaletteId.Value;
            }
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
                repositoryModel.SyncedAt.ToUniversalTime(),
                repositoryModel.CreatedAt,
                repositoryModel.UpdatedAt
                );
        }

        public static StampPaletteSubscriptionWithStampPalette ToStampPaletteSubscriptionWithStampPalette(this (RepoStampPaletteSubscription, (RepoStampPalette, IEnumerable<RepoStampPaletteSubscription>)) repositoryModel)
        {
            var (subscription, palette) = repositoryModel;
            return new StampPaletteSubscriptionWithStampPalette(
                subscription.Id,
                subscription.UserId,
                palette.ToStampPalette(),
                subscription.CopiedPaletteId,
                subscription.SyncedAt.ToUniversalTime(),
                subscription.CreatedAt,
                subscription.UpdatedAt
                );
        }
    }
}
