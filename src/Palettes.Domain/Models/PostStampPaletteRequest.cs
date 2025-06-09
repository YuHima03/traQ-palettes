namespace Palettes.Domain.Models
{
    public readonly record struct PostStampPaletteRequest(
        Guid Id,
        Guid UserId,
        bool IsPublic
        );
}
