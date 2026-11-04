using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("user_roles")]
public class UserRole
{
    [Key] [Column("id")] public int Id { get; set; }
    [Required] [MaxLength(50)] [Column("name")] public string Name { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
}