using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SchedulesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules() => 
        await _context.Schedules.Include(s => s.Section).Include(s => s.Facility).ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Schedule>> PostSchedule(Schedule schedule)
    {
        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSchedules), new { id = schedule.Id }, schedule);
    }
}
