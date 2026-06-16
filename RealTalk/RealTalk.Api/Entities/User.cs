using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTalk.Api.Entities
{
    [Table("USER", Schema = "dbo")]
    public class User
    {
        [Key]
        [Column("ID")]
        [StringLength(50)]
        public Guid Id { get; set; }

        [Required]
        [Column("USERNAME")]
        [StringLength(20)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        [Column("EMAIL")]
        [StringLength(255)]
        public required string Email { get; set; }

        [Required]
        [Column("PASSWORD_HASH")]
        [StringLength(255)]
        public required string PasswordHash { get; set; }

        [Column("REFRESH_TOKEN")]
        [StringLength(500)]
        public string? RefreshToken { get; set; }

        [Column("REFRESH_TOKEN_EXPIRY")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}