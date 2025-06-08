namespace Palettes.Domain.Models
{
    public sealed record class StampPaletteRef(
        Guid Id,
        Guid UserId,
        bool IsPublic,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
        );
}
