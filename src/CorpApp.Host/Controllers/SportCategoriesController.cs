using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SportCategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SportCategoriesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SportCategory>>> GetCategories() => await _context.SportCategories.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<SportCategory>> PostCategory(SportCategory category)
    {
        _context.SportCategories.Add(category);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
    }
}
