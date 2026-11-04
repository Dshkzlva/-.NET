using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("users")]
public class User
{
    [Key] [Column("id")] public Guid Id { get; set; } = Guid.NewGuid(); // UUID из документа
    [Required] [MaxLength(100)] [Column("email")] public string Email { get; set; } = null!;
    [Required] [MaxLength(255)] [Column("password_hash")] public string PasswordHash { get; set; } = null!;
    [Required] [MaxLength(100)] [Column("first_name")] public string FirstName { get; set; } = null!;
    [Required] [MaxLength(100)] [Column("last_name")] public string LastName { get; set; } = null!;
    
    [Required] [Column("role_id")] public int RoleId { get; set; }
    [ForeignKey(nameof(RoleId))] public UserRole Role { get; set; } = null!;

    public StudentProfile? StudentProfile { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<News> News { get; set; } = new List<News>();
}