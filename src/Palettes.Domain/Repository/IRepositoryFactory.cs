namespace Palettes.Domain.Repository
{
    public interface IRepositoryFactory
    {
        public IRepository CreateRepository()
        {
            return CreateRepositoryAsync(default).AsTask().GetAwaiter().GetResult();
        }

        public ValueTask<IRepository> CreateRepositoryAsync(CancellationToken ct);
    }
}
