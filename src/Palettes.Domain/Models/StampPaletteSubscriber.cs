namespace Palettes.Domain.Models
{
    public sealed record class StampPaletteSubscriber(
        Guid SubscriptionId,
        Guid UserId,
        DateTimeOffset CreatedAt
        );
}
