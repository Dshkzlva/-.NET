using Microsoft.EntityFrameworkCore;
using CorpApp.Entities.Models;

namespace CorpApp.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<User> Users => Set<User>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<SportCategory> SportCategories => Set<SportCategory>();
    public DbSet<Coach> Coaches => Set<Coach>();
    public DbSet<Facility> Facilities => Set<Facility>();
    public DbSet<SportsSection> SportsSections => Set<SportsSection>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<News> News => Set<News>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // Уникальные поля (Ограничения UNIQUE)
        mb.Entity<User>().HasIndex(u => u.Email).IsUnique();
        mb.Entity<UserRole>().HasIndex(r => r.Name).IsUnique();
        mb.Entity<SportsSection>().HasIndex(s => s.Name).IsUnique();
    }
}