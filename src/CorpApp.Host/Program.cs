using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CorpApp.Migrations")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ==========================================
// USER ROLES
// ==========================================
app.MapGet("/api/userroles", async (AppDbContext db) => await db.UserRoles.ToListAsync())
   .WithName("GetUserRoles");

app.MapPost("/api/userroles", async (UserRole role, AppDbContext db) =>
{
    db.UserRoles.Add(role);
    await db.SaveChangesAsync();
    return Results.Created($"/api/userroles/{role.Id}", role);
}).WithName("CreateUserRole");

// ==========================================
// SPORT CATEGORIES
// ==========================================
app.MapGet("/api/sportcategories", async (AppDbContext db) => await db.SportCategories.ToListAsync())
   .WithName("GetSportCategories");

app.MapPost("/api/sportcategories", async (SportCategory category, AppDbContext db) =>
{
    db.SportCategories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/api/sportcategories/{category.Id}", category);
}).WithName("CreateSportCategory");

// ==========================================
// FACILITIES
// ==========================================
app.MapGet("/api/facilities", async (AppDbContext db) => await db.Facilities.ToListAsync())
   .WithName("GetFacilities");

app.MapPost("/api/facilities", async (Facility facility, AppDbContext db) =>
{
    db.Facilities.Add(facility);
    await db.SaveChangesAsync();
    return Results.Created($"/api/facilities/{facility.Id}", facility);
}).WithName("CreateFacility");

// ==========================================
// COACHES
// ==========================================
app.MapGet("/api/coaches", async (AppDbContext db) => await db.Coaches.ToListAsync())
   .WithName("GetCoaches");

app.MapGet("/api/coaches/{id:int}", async (int id, AppDbContext db) =>
{
    var coach = await db.Coaches.FindAsync(id);
    return coach is not null ? Results.Ok(coach) : Results.NotFound();
}).WithName("GetCoachById");

app.MapPost("/api/coaches", async (Coach coach, AppDbContext db) =>
{
    db.Coaches.Add(coach);
    await db.SaveChangesAsync();
    return Results.Created($"/api/coaches/{coach.Id}", coach);
}).WithName("CreateCoach");

// ==========================================
// USERS
// ==========================================
app.MapGet("/api/users", async (AppDbContext db) => await db.Users.ToListAsync())
   .WithName("GetUsers");

app.MapPost("/api/users", async (User user, AppDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/api/users/{user.Id}", user);
}).WithName("CreateUser");

// ==========================================
// SPORTS SECTIONS
// ==========================================
app.MapGet("/api/sportssections", async (AppDbContext db) => await db.SportsSections.ToListAsync())
   .WithName("GetSportsSections");

app.MapPost("/api/sportssections", async (SportsSection section, AppDbContext db) =>
{
    db.SportsSections.Add(section);
    await db.SaveChangesAsync();
    return Results.Created($"/api/sportssections/{section.Id}", section);
}).WithName("CreateSportsSection");

// ==========================================
// SCHEDULES
// ==========================================
app.MapGet("/api/schedules", async (AppDbContext db) => await db.Schedules.ToListAsync())
   .WithName("GetSchedules");

app.MapPost("/api/schedules", async (Schedule schedule, AppDbContext db) =>
{
    db.Schedules.Add(schedule);
    await db.SaveChangesAsync();
    return Results.Created($"/api/schedules/{schedule.Id}", schedule);
}).WithName("CreateSchedule");

// ==========================================
// ENROLLMENTS
// ==========================================
app.MapGet("/api/enrollments", async (AppDbContext db) => await db.Enrollments.ToListAsync())
   .WithName("GetEnrollments");

app.MapPost("/api/enrollments", async (Enrollment enrollment, AppDbContext db) =>
{
    db.Enrollments.Add(enrollment);
    await db.SaveChangesAsync();
    return Results.Created($"/api/enrollments/{enrollment.Id}", enrollment);
}).WithName("CreateEnrollment");

// ==========================================
// NEWS
// ==========================================
app.MapGet("/api/news", async (AppDbContext db) => await db.News.ToListAsync())
   .WithName("GetNews");

app.MapPost("/api/news", async (News news, AppDbContext db) =>
{
    db.News.Add(news);
    await db.SaveChangesAsync();
    return Results.Created($"/api/news/{news.Id}", news);
}).WithName("CreateNews");

// ==========================================
// STUDENT PROFILES
// ==========================================
app.MapGet("/api/studentprofiles", async (AppDbContext db) => await db.StudentProfiles.ToListAsync())
   .WithName("GetStudentProfiles");

app.MapPost("/api/studentprofiles", async (StudentProfile profile, AppDbContext db) =>
{
    db.StudentProfiles.Add(profile);
    await db.SaveChangesAsync();
    return Results.Created($"/api/studentprofiles/{profile.Id}", profile);
}).WithName("CreateStudentProfile");

app.Run();
