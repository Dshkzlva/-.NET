using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NewsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetNews() => await _context.News.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<News>> PostNews(News news)
    {
        _context.News.Add(news);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetNews), new { id = news.Id }, news);
    }
}
