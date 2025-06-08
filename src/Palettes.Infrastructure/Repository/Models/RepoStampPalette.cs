using System.ComponentModel.DataAnnotations.Schema;

namespace Palettes.Infrastructure.Repository.Models
{
    [Table("stamp_palettes")]
    sealed class RepoStampPalette
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("is_public")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool IsPublic { get; set; }

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
