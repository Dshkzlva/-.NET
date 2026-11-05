using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoachesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoachesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Coach>>> GetCoaches() => await _context.Coaches.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Coach>> GetCoach(int id)
    {
        var coach = await _context.Coaches.FindAsync(id);
        if (coach == null) return NotFound();
        return coach;
    }

    [HttpPost]
    public async Task<ActionResult<Coach>> PostCoach(Coach coach)
    {
        _context.Coaches.Add(coach);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCoach), new { id = coach.Id }, coach);
    }
}
