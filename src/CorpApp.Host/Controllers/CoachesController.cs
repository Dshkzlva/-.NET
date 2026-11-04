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

    public CoachesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/coaches
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Coach>>> GetCoaches()
    {
        return await _context.Coaches.ToListAsync();
    }

    // GET: api/coaches/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Coach>> GetCoach(int id)
    {
        var coach = await _context.Coaches.FindAsync(id);
        if (coach == null) return NotFound();
        return coach;
    }

    // POST: api/coaches
    [HttpPost]
    public async Task<ActionResult<Coach>> PostCoach(Coach coach)
    {
        _context.Coaches.Add(coach);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCoach), new { id = coach.Id }, coach);
    }
}
