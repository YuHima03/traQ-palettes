namespace Palettes.Domain.Models
{
    public readonly record struct PostStampPaletteRequest(
        Guid UserId,
        bool IsPublic
        );
}
