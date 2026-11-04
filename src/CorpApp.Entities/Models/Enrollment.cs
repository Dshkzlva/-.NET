using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("enrollments")]
public class Enrollment
{
    [Key] [Column("id")] public int Id { get; set; }
    [Required] [Column("user_id")] public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))] public User User { get; set; } = null!;

    [Required] [Column("section_id")] public int SectionId { get; set; }
    [ForeignKey(nameof(SectionId))] public SportsSection Section { get; set; } = null!;

    [Column("enrolled_at")] public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    [MaxLength(20)] [Column("status")] public string Status { get; set; } = "active";
}