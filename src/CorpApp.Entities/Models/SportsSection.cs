using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("sports_sections")]
public class SportsSection
{
    [Key] [Column("id")] public int Id { get; set; }
    [Required] [MaxLength(150)] [Column("name")] public string Name { get; set; } = null!;
    
    [Required] [Column("category_id")] public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))] public SportCategory Category { get; set; } = null!;

    [Column("coach_id")] public int? CoachId { get; set; }
    [ForeignKey(nameof(CoachId))] public Coach? Coach { get; set; }

    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}