namespace Palettes.Domain.Models
{
    public readonly record struct PostStampPaletteSubscriptionRequest(
        Guid UserId,
        Guid StampPaletteId,
        DateTimeOffset SyncedAt
        );
}
