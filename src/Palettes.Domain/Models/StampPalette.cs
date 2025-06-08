namespace Palettes.Domain.Models
{
    public sealed record class StampPalette(
        Guid Id,
        Guid UserId,
        bool IsPublic,
        StampPaletteSubscription[] Subscribers,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
        );
}
