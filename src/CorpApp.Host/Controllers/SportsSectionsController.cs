using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SportsSectionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SportsSectionsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SportsSection>>> GetSections() => 
        await _context.SportsSections.Include(s => s.Category).Include(s => s.Coach).ToListAsync();

    [HttpPost]
    public async Task<ActionResult<SportsSection>> PostSection(SportsSection section)
    {
        _context.SportsSections.Add(section);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSections), new { id = section.Id }, section);
    }
}
