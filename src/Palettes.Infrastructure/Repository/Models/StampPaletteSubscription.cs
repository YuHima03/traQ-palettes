using System.ComponentModel.DataAnnotations.Schema;

namespace Palettes.Infrastructure.Repository.Models
{
    [Table("stamp_palette_subscriptions")]
    sealed class StampPaletteSubscription
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("palette_id")]
        public Guid PaletteId { get; set; }

        [Column("copied_palette_id")]
        public Guid CopiedPaletteId { get; set; }

        [Column("synced_at")]
        public DateTime SyncedAt { get; set; }

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
