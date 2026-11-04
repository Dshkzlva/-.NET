using CorpApp.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Entities;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<StudentProfile> StudentProfiles { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<SportCategory> SportCategories { get; set; }
    public DbSet<SportsSection> SportsSections { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<News> News { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Уникальные индексы на основе логики предметной области
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserRole>().HasIndex(r => r.Name).IsUnique();
        modelBuilder.Entity<SportCategory>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Facility>().HasIndex(f => f.Name).IsUnique();
    }
}
