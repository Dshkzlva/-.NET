using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("student_profiles")]
public class StudentProfile
{
    [Key] [Column("id")] public int Id { get; set; }
    [Required] [Column("user_id")] public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))] public User User { get; set; } = null!;
    [MaxLength(50)] [Column("group_number")] public string? GroupNumber { get; set; }
}