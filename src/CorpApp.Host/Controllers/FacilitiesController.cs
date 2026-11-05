using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FacilitiesController : ControllerBase
{
    private readonly AppDbContext _context;

    public FacilitiesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Facility>>> GetFacilities() => await _context.Facilities.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Facility>> PostFacility(Facility facility)
    {
        _context.Facilities.Add(facility);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetFacilities), new { id = facility.Id }, facility);
    }
}
