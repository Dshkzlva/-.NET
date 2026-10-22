using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("user_in_roles")]
public class UserInRole
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

    // FK на Roles.Id
    [Required]
    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = null!;

    // NOT NULL, по умолчанию — текущее время
    [Required]
    [Column("create_date")]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
}