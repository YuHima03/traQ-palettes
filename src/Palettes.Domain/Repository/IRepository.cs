namespace Palettes.Domain.Repository
{
    public interface IRepository :
        IRepositoryBase,
        IStampPalettesRepository,
        IStampPaletteSubscriptionsRepository;
}
