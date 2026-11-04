using CorpApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CorpApp.Host;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== ПРОВЕРКА ТАБЛИЦ В БАЗЕ ДАННЫХ ===\n");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Port=5433;Database=corpdb;Username=postgres;Password=postgres")
            .Options;

        using var db = new AppDbContext(options);

        try
        {
            if (!await db.Database.CanConnectAsync())
            {
                Console.WriteLine(" Ошибка: Не удалось подключиться к базе. Проверьте Docker.");
                return;
            }

            // Получаем список всех таблиц через SQL-запрос к системному каталогу PostgreSQL
            var tableNames = await db.Database
                .SqlQueryRaw<string>("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE' ORDER BY table_name")
                .ToListAsync();

            Console.WriteLine($"Найдено таблиц в схеме 'public': {tableNames.Count}\n");

            foreach (var tableName in tableNames)
            {
                Console.WriteLine($" Таблица: {tableName}");

                // Получаем количество строк в таблице
                var count = await db.Database
                    .SqlQueryRaw<long>($"SELECT count(*) as \"Value\" FROM \"{tableName}\"")
                    .FirstAsync();

                Console.WriteLine($"   Записей: {count}");

                // Получаем колонки и их типы
                var columns = await db.Database
                    .SqlQueryRaw<ColumnInfo>($@"
                        SELECT column_name as ""Name"", data_type as ""Type"", is_nullable as ""IsNullable""
                        FROM information_schema.columns 
                        WHERE table_name = '{tableName}' AND table_schema = 'public'
                        ORDER BY ordinal_position")
                    .ToListAsync();

                foreach (var col in columns)
                {
                    var nullStr = col.IsNullable == "YES" ? "NULL" : "NOT NULL";
                    Console.WriteLine($"      - {col.Name,-20} | {col.Type,-25} | {nullStr}");
                }
                Console.WriteLine();
            }
            
            Console.WriteLine(" ПРОВЕРКА ЗАВЕРШЕНА УСПЕШНО!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Ошибка: {ex.Message}");
        }
    }
}

// Вспомогательный класс для чтения метаданных колонок
public class ColumnInfo
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string IsNullable { get; set; } = null!;
}