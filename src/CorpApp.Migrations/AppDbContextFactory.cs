using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CorpApp.Entities; // <-- Обязательно!

namespace CorpApp.Migrations; // <-- Изменили на Migrations

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            // Ваша строка подключения (порт 5433, база corpdb)
            .UseNpgsql("Host=localhost;Port=5433;Database=corpdb;Username=postgres;Password=postgres")
            .Options;

        return new AppDbContext(options);
    }
}