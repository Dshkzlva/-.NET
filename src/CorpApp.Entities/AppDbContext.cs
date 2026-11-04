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
        // Уникальные индексы
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserRole>().HasIndex(r => r.Name).IsUnique();
        modelBuilder.Entity<SportCategory>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Facility>().HasIndex(f => f.Name).IsUnique();
        modelBuilder.Entity<Schedule>().HasIndex(s => new { s.SectionId, s.FacilityId, s.DayOfWeek, s.StartTime }).IsUnique();
        modelBuilder.Entity<Enrollment>().HasIndex(e => new { e.UserId, e.SectionId }).IsUnique();

        // Связи
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.StudentProfile)
            .WithOne(sp => sp.User)
            .HasForeignKey<StudentProfile>(sp => sp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SportsSection>()
            .HasOne(ss => ss.Category)
            .WithMany(c => c.Sections)
            .HasForeignKey(ss => ss.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SportsSection>()
            .HasOne(ss => ss.Coach)
            .WithMany(c => c.Sections)
            .HasForeignKey(ss => ss.CoachId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Section)
            .WithMany(ss => ss.Schedules)
            .HasForeignKey(s => s.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Facility)
            .WithMany(f => f.Schedules)
            .HasForeignKey(s => s.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.User)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Section)
            .WithMany(ss => ss.Enrollments)
            .HasForeignKey(e => e.SectionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<News>()
            .HasOne(n => n.Author)
            .WithMany(u => u.News)
            .HasForeignKey(n => n.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
