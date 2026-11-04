using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("coaches")]
public class Coach
{
    [Key] [Column("id")] public int Id { get; set; }
    [Required] [MaxLength(100)] [Column("first_name")] public string FirstName { get; set; } = null!;
    [Required] [MaxLength(100)] [Column("last_name")] public string LastName { get; set; } = null!;
    public ICollection<SportsSection> Sections { get; set; } = new List<SportsSection>();
}