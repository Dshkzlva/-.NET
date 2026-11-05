using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserRolesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRole>>> GetRoles() => await _context.UserRoles.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<UserRole>> PostRole(UserRole role)
    {
        _context.UserRoles.Add(role);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, role);
    }
}
