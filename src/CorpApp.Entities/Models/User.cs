using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    // NOT NULL, UNIQUE
    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    [Column("middle_name")]
    public string? MiddleName { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Column("last_activity_date")]
    public DateTime? LastActivityDate { get; set; }

    // Навигация
    public ICollection<UserPassword> Passwords { get; set; } = new List<UserPassword>();
    public ICollection<UserInRole> UserRoles { get; set; } = new List<UserInRole>();
}