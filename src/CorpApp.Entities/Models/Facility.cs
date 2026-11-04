using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("facilities")]
public class Facility
{
    [Key] [Column("id")] public int Id { get; set; }
    [Required] [MaxLength(100)] [Column("name")] public string Name { get; set; } = null!;
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}