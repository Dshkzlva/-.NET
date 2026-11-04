using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CorpApp.Entities;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=corpdb;Username=postgres;Password=postgres",
            b => b.MigrationsAssembly("CorpApp.Migrations"));
        return new AppDbContext(optionsBuilder.Options);
    }
}
