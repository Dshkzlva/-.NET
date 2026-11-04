using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorpApp.Entities.Models;

[Table("news")]
public class News
{
    [Key] [Column("id")] public int Id { get; set; }
    [Column("author_id")] public Guid? AuthorId { get; set; } // SET NULL при удалении
    [ForeignKey(nameof(AuthorId))] public User? Author { get; set; }

    [Required] [MaxLength(150)] [Column("title")] public string Title { get; set; } = null!;
    [Required] [Column("content")] public string Content { get; set; } = null!;
    [Column("published_at")] public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
}