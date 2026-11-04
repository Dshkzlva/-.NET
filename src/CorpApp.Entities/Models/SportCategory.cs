using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("sport_categories")]
public class SportCategory
{
    [Key] [Column("id")] public int Id { get; set; }
    [Required] [MaxLength(100)] [Column("name")] public string Name { get; set; } = null!;
    public ICollection<SportsSection> Sections { get; set; } = new List<SportsSection>();
}