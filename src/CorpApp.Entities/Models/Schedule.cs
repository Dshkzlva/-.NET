using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("schedules")]
public class Schedule
{
    [Key] [Column("id")] public int Id { get; set; }
    [Required] [Column("section_id")] public int SectionId { get; set; }
    [ForeignKey(nameof(SectionId))] public SportsSection Section { get; set; } = null!;

    [Required] [Column("facility_id")] public int FacilityId { get; set; }
    [ForeignKey(nameof(FacilityId))] public Facility Facility { get; set; } = null!;

    [Required] [MaxLength(12)] [Column("day_of_week")] public string DayOfWeek { get; set; } = null!;
    [Required] [Column("start_time")] public TimeSpan StartTime { get; set; }
    [Required] [Column("end_time")] public TimeSpan EndTime { get; set; }
}