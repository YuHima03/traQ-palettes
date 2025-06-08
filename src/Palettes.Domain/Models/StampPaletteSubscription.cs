namespace Palettes.Domain.Models
{
    public sealed record class StampPaletteSubscription(
        Guid Id,
        Guid UserId,
        Guid StampPaletteId,
        Guid CopiedStampPaletteId,
        DateTimeOffset SyncedAt,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
        );
}
