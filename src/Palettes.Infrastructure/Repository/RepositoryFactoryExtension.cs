using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Palettes.Domain.Repository;

namespace Palettes.Infrastructure.Repository
{
    public static class RepositoryFactoryExtension
    {
        public static IServiceCollection AddRepositoryFactory(this IServiceCollection services, Action<IServiceProvider, DbContextOptionsBuilder> configureOptionsBuilder, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.AddDbContextFactory<Repository>(configureOptionsBuilder, lifetime);
            services.TryAdd(new ServiceDescriptor(
                typeof(IRepositoryFactory),
                sp => new RepositoryFactory(sp.GetRequiredService<IDbContextFactory<Repository>>()),
                lifetime));
            return services;
        }
    }
}
