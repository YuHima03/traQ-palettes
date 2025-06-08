namespace Palettes.Domain.Models
{
    public readonly record struct UpdateStampPaletteSubscriptionRequest(
        DateTimeOffset SyncedAt
        );
}
