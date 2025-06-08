using Microsoft.EntityFrameworkCore;
using Palettes.Domain.Repository;
using Palettes.Infrastructure.Repository.Models;

namespace Palettes.Infrastructure.Repository
{
    public sealed partial class Repository(DbContextOptions<Repository> options) : DbContext(options), IRepository
    {
        DbSet<RepoStampPalette> StampPalettes { get; set; }

        DbSet<RepoStampPaletteSubscription> StampPaletteSubscriptions { get; set; }
    }
}
