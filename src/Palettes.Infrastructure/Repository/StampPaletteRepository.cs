using Microsoft.EntityFrameworkCore;
using Palettes.Domain.Models;
using Palettes.Domain.Repository;
using Palettes.Infrastructure.Repository.Models;

namespace Palettes.Infrastructure.Repository
{
    public sealed partial class Repository : IStampPalettesRepository
    {
        public async ValueTask<StampPalette[]> GetPublicStampPalettesAsync(CancellationToken ct)
        {
            return await StampPalettes
                .Where(sp => sp.IsPublic)
                .JoinSubscriptions(StampPaletteSubscriptions)
                .Select(v => v.ToStampPalette())
                .ToArrayAsync(ct);
        }

        public async ValueTask<StampPalette> GetStampPaletteAsync(Guid id, CancellationToken ct)
        {
            var sp = await StampPalettes
                .Where(p => p.Id == id)
                .JoinSubscriptions(StampPaletteSubscriptions)
                .FirstAsync(ct);
            return sp.ToStampPalette();
        }

        public async ValueTask<StampPalette[]> GetUserStampPalettesAsync(Guid userId, bool includePrivate, CancellationToken ct)
        {
            return await StampPalettes
                .Where(includePrivate switch
                {
                    true => sp => sp.UserId == userId,
                    false => sp => sp.UserId == userId && sp.IsPublic
                })
                .JoinSubscriptions(StampPaletteSubscriptions)
                .Select(v => v.ToStampPalette())
                .ToArrayAsync(ct);
        }

        public async ValueTask<StampPalette> PostStampPaletteAsync(PostStampPaletteRequest request, CancellationToken ct)
        {
            RepoStampPalette repoModel = new()
            {
                Id = Guid.CreateVersion7(),
                UserId = request.UserId,
                IsPublic = request.IsPublic
            };
            StampPalettes.Add(repoModel);
            await SaveChangesAsync(ct);
            return repoModel.ToStampPalette();
        }

        public async ValueTask<StampPalette?> TryGetStampPaletteAsync(Guid id, CancellationToken ct)
        {
            var sp = await StampPalettes
                .Where(p => p.Id == id)
                .JoinSubscriptions(StampPaletteSubscriptions)
                .Select(v => v.ToTuple())
                .FirstOrDefaultAsync(ct);
            return sp?.ToValueTuple().ToStampPalette();
        }

        public async ValueTask<StampPalette> UpdateStampPaletteAsync(Guid id, UpdateStampPaletteRequest request, CancellationToken ct)
        {
            var sp = await StampPalettes
                .Where(p => p.Id == id)
                .JoinSubscriptions(StampPaletteSubscriptions)
                .FirstAsync(ct);
            sp.Item1.IsPublic = request.IsPublic;
            await SaveChangesAsync(ct);
            return sp.ToStampPalette();
        }
    }

    static class StampPaletteRepositoryExtension
    {
        public static IQueryable<(RepoStampPalette, IEnumerable<RepoStampPaletteSubscription>)> JoinSubscriptions(this IQueryable<RepoStampPalette> q, IEnumerable<RepoStampPaletteSubscription> subscriptions)
        {
            return q.GroupJoin(
                subscriptions,
                sp => sp.Id,
                subs => subs.PaletteId,
                (sp, subs) => ValueTuple.Create(sp, subs)
                );
        }

        public static StampPalette ToStampPalette(this RepoStampPalette repositoryModel)
        {
            return new StampPalette(
                repositoryModel.Id,
                repositoryModel.UserId,
                repositoryModel.IsPublic,
                [],
                repositoryModel.CreatedAt,
                repositoryModel.UpdatedAt
            );
        }

        public static StampPalette ToStampPalette(this (RepoStampPalette, IEnumerable<RepoStampPaletteSubscription>) repositoryModel)
        {
            var (sp, subs) = repositoryModel;
            return new StampPalette(
                sp.Id,
                sp.UserId,
                sp.IsPublic,
                [.. subs.Select(ToStampPaletteSubscriber)],
                sp.CreatedAt,
                sp.UpdatedAt
            );
        }

        public static StampPaletteSubscriber ToStampPaletteSubscriber(this RepoStampPaletteSubscription repositoryModel)
        {
            return new StampPaletteSubscriber(
                repositoryModel.UserId,
                repositoryModel.CreatedAt
            );
        }
    }
}
