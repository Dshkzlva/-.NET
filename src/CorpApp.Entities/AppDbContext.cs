using Microsoft.EntityFrameworkCore;
using CorpApp.Entities.Models;

namespace CorpApp.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserPassword> UserPasswords => Set<UserPassword>();
    public DbSet<UserInRole> UserInRoles => Set<UserInRole>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // UNIQUE на Users.Name
        mb.Entity<User>()
          .HasIndex(u => u.Name)
          .IsUnique();

        // UNIQUE на Roles.Name
        mb.Entity<Role>()
          .HasIndex(r => r.Name)
          .IsUnique();

        // UNIQUE-пара (UserId, RoleId) — нельзя дважды выдать одну роль
        mb.Entity<UserInRole>()
          .HasIndex(ur => new { ur.UserId, ur.RoleId })
          .IsUnique();

        // Связь User 1 — * UserPassword
        mb.Entity<UserPassword>()
          .HasOne(p => p.User)
          .WithMany(u => u.Passwords)
          .HasForeignKey(p => p.UserId)
          .OnDelete(DeleteBehavior.Cascade);

        // Связь User 1 — * UserInRole
        mb.Entity<UserInRole>()
          .HasOne(ur => ur.User)
          .WithMany(u => u.UserRoles)
          .HasForeignKey(ur => ur.UserId)
          .OnDelete(DeleteBehavior.Cascade);

        // Связь Role 1 — * UserInRole
        mb.Entity<UserInRole>()
          .HasOne(ur => ur.Role)
          .WithMany(r => r.UserRoles)
          .HasForeignKey(ur => ur.RoleId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}