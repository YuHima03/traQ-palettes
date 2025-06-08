using Microsoft.EntityFrameworkCore;
using Palettes.Domain.Repository;

namespace Palettes.Infrastructure.Repository
{
    public sealed class RepositoryFactory(IDbContextFactory<Repository> factory) : IRepositoryFactory
    {
        public IRepository CreateRepository()
        {
            return factory.CreateDbContext();
        }

        public async ValueTask<IRepository> CreateRepositoryAsync(CancellationToken ct)
        {
            return await factory.CreateDbContextAsync(ct);
        }
    }
}
