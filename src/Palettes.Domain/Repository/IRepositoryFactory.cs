namespace Palettes.Domain.Repository
{
    public interface IRepositoryFactory
    {
        public IRepository CreateRepository();
        public ValueTask<IRepository> CreateRepositoryAsync(CancellationToken ct);
    }
}
