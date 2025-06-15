namespace Palettes.Domain.Models
{
    public sealed record class StampPaletteSubscriptionWithStampPalette(
        Guid Id,
        Guid UserId,
        StampPalette StampPalette,
        Guid CopiedStampPaletteId,
        DateTimeOffset SyncedAt,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
        );
}
