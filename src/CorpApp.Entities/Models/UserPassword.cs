using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("user_passwords")]
public class UserPassword
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    // FK на Users.Id
    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    // NOT NULL — хэш пароля
    [Required]
    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; } = null!;
}