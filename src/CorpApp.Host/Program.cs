using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CorpApp.Host;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowVueApp", policy => {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader().AllowAnyMethod();
    });
});

// JWT Authentication
var jwtSecret = builder.Configuration["JwtSettings:SecretKey"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CorpApp.Migrations")));

builder.Services.AddScoped<JwtService>();

var app = builder.Build();

app.UseCors("AllowVueApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

// ============ AUTH ENDPOINT (исправленный!) ============
app.MapPost("/api/auth/login", async ([FromBody] LoginRequest request, AppDbContext db, JwtService jwtService) => {
    if (string.IsNullOrEmpty(request.Email)) {
        return Results.BadRequest(new { error = "Email is required" });
    }

    var user = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
    if (user is null) {
        return Results.Unauthorized();
    }

    var token = jwtService.GenerateToken(user);
    return Results.Ok(new {
        token,
        user = new {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.RoleId
        }
    });
}).WithName("Login").Produces(200).Produces(401);

// ============ PUBLIC ENDPOINTS ============
app.MapGet("/api/userroles", async (AppDbContext db) => await db.UserRoles.ToListAsync());
app.MapGet("/api/sportcategories", async (AppDbContext db) => await db.SportCategories.ToListAsync());
app.MapGet("/api/facilities", async (AppDbContext db) => await db.Facilities.ToListAsync());
app.MapGet("/api/coaches", async (AppDbContext db) => await db.Coaches.ToListAsync());
app.MapGet("/api/sportssections", async (AppDbContext db) => await db.SportsSections.ToListAsync());
app.MapGet("/api/schedules", async (AppDbContext db) => await db.Schedules.ToListAsync());
app.MapGet("/api/news", async (AppDbContext db) => await db.News.ToListAsync());

// ============ PROTECTED ENDPOINTS ============
app.MapGet("/api/users", async (AppDbContext db) => await db.Users.ToListAsync()).RequireAuthorization();
app.MapGet("/api/studentprofiles", async (AppDbContext db) => await db.StudentProfiles.ToListAsync()).RequireAuthorization();
app.MapGet("/api/enrollments", async (AppDbContext db) => await db.Enrollments.ToListAsync()).RequireAuthorization();

app.MapPost("/api/enrollments", async (Enrollment e, AppDbContext db) => {
    db.Enrollments.Add(e);
    await db.SaveChangesAsync();
    return Results.Created($"/api/enrollments/{e.Id}", e);
}).RequireAuthorization();

app.MapDelete("/api/enrollments/{id:int}", async (int id, AppDbContext db) => {
    var e = await db.Enrollments.FindAsync(id);
    if (e is null) return Results.NotFound();
    db.Enrollments.Remove(e);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

app.Run();

// ============ DTO для логина (обычный класс, не record!) ============
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
}
