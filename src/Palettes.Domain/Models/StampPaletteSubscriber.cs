namespace Palettes.Domain.Models
{
    public sealed record class StampPaletteSubscriber(
        Guid UserId,
        DateTimeOffset CreatedAt
        );
}
