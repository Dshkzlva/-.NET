using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDto { Id = u.Id, Name = u.Name, FirstName = u.FirstName, LastName = u.LastName })
                .ToListAsync();
            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(new UserDto { Id = user.Id, Name = user.Name, FirstName = user.FirstName, LastName = user.LastName });
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                LastActivityDate = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new UserDto { Id = user.Id, Name = user.Name, FirstName = user.FirstName, LastName = user.LastName });
        }

        public class UserDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }

        public class CreateUserDto
        {
            public string Name { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }
    }
}
