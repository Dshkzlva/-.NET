using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentProfilesController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentProfilesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentProfile>>> GetProfiles() => 
        await _context.StudentProfiles.Include(p => p.User).ToListAsync();

    [HttpPost]
    public async Task<ActionResult<StudentProfile>> PostProfile(StudentProfile profile)
    {
        _context.StudentProfiles.Add(profile);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProfiles), new { id = profile.Id }, profile);
    }
}
