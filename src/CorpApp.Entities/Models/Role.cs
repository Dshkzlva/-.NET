using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("roles")]
public class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    // NOT NULL + UNIQUE
    [Required]
    [MaxLength(50)]
    [Column("name")]
    public string Name { get; set; } = null!;

    // Навигация: одна роль — много записей в UserInRoles
    public ICollection<UserInRole> UserRoles { get; set; } = new List<UserInRole>();
}