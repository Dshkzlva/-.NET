using CorpApp.Entities;
using CorpApp.Entities.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CorpApp.Migrations")));

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

// Автоматическое применение миграций и наполнение БД данными из макета
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.SportsSections.AnyAsync())
    {
        var roleStudent = new UserRole { Name = "student" };
        var roleTeacher = new UserRole { Name = "teacher" };
        db.UserRoles.AddRange(roleStudent, roleTeacher);

        var catTeam = new SportCategory { Name = "Командные игры" };
        var catWater = new SportCategory { Name = "Водный спорт" };
        var catRacket = new SportCategory { Name = "Ракеточные виды" };
        var catCombat = new SportCategory { Name = "Единоборства" };
        var catGeneral = new SportCategory { Name = "Общая подготовка" };
        var catMind = new SportCategory { Name = "Интеллектуальный спорт" };
        db.SportCategories.AddRange(catTeam, catWater, catRacket, catCombat, catGeneral, catMind);

        var coach1 = new Coach { FirstName = "Сергей Александрович", LastName = "Иванов" };
        var coach2 = new Coach { FirstName = "Дмитрий Игоревич", LastName = "Кузнецов" };
        var coach3 = new Coach { FirstName = "Анна Игоревна", LastName = "Петрова" };
        var coach4 = new Coach { FirstName = "Алексей Владимирович", LastName = "Смирнов" };
        db.Coaches.AddRange(coach1, coach2, coach3, coach4);

        var hall1 = new Facility { Name = "Зал №1" };
        var field = new Facility { Name = "Спортивная площадка" };
        var pool = new Facility { Name = "Бассейн ЮУрГУ" };
        db.Facilities.AddRange(hall1, field, pool);

        await db.SaveChangesAsync();

        var studentUser = new User
        {
            Email = "ivanov@susu.ru",
            PasswordHash = "12345",
            FirstName = "Иван",
            LastName = "Иванов",
            RoleId = roleStudent.Id
        };
        var teacherUser = new User
        {
            Email = "sokolov@susu.ru",
            PasswordHash = "12345",
            FirstName = "П.П.",
            LastName = "Соколов",
            RoleId = roleTeacher.Id
        };
        db.Users.AddRange(studentUser, teacherUser);
        await db.SaveChangesAsync();

        db.StudentProfiles.Add(new StudentProfile { UserId = studentUser.Id, GroupNumber = "КЭ-305" });

        var secFootball = new SportsSection { Name = "Футбол", CategoryId = catTeam.Id, CoachId = coach1.Id };
        var secBasketball = new SportsSection { Name = "Баскетбол", CategoryId = catTeam.Id, CoachId = coach2.Id };
        var secVolleyball = new SportsSection { Name = "Волейбол", CategoryId = catTeam.Id, CoachId = coach3.Id };
        var secSwimming = new SportsSection { Name = "Плавание", CategoryId = catWater.Id, CoachId = coach4.Id };
        db.SportsSections.AddRange(
            secFootball,
            secBasketball,
            secVolleyball,
            secSwimming,
            new SportsSection { Name = "Теннис", CategoryId = catRacket.Id, CoachId = coach1.Id },
            new SportsSection { Name = "Борьба", CategoryId = catCombat.Id, CoachId = coach2.Id },
            new SportsSection { Name = "Йога", CategoryId = catGeneral.Id, CoachId = coach3.Id },
            new SportsSection { Name = "Шахматы", CategoryId = catMind.Id, CoachId = coach4.Id }
        );
        await db.SaveChangesAsync();

        db.Schedules.AddRange(
            new Schedule { SectionId = secFootball.Id, FacilityId = field.Id, DayOfWeek = "Вторник", StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 30, 0) },
            new Schedule { SectionId = secFootball.Id, FacilityId = field.Id, DayOfWeek = "Четверг", StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 30, 0) },
            new Schedule { SectionId = secFootball.Id, FacilityId = field.Id, DayOfWeek = "Пятница", StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 30, 0) },
            new Schedule { SectionId = secBasketball.Id, FacilityId = hall1.Id, DayOfWeek = "Понедельник", StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(19, 30, 0) },
            new Schedule { SectionId = secSwimming.Id, FacilityId = pool.Id, DayOfWeek = "Среда", StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 0, 0) }
        );

        db.Enrollments.Add(new Enrollment { UserId = studentUser.Id, SectionId = secFootball.Id, Status = "active" });
        await db.SaveChangesAsync();
    }
}

// ==========================================
// API ENDPOINTS
// ==========================================
app.MapGet("/api/userroles", async (AppDbContext db) => await db.UserRoles.ToListAsync());
app.MapPost("/api/userroles", async (UserRole role, AppDbContext db) =>
{
    db.UserRoles.Add(role);
    await db.SaveChangesAsync();
    return Results.Created($"/api/userroles/{role.Id}", role);
});

app.MapGet("/api/sportcategories", async (AppDbContext db) => await db.SportCategories.ToListAsync());
app.MapPost("/api/sportcategories", async (SportCategory category, AppDbContext db) =>
{
    db.SportCategories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/api/sportcategories/{category.Id}", category);
});

app.MapGet("/api/facilities", async (AppDbContext db) => await db.Facilities.ToListAsync());
app.MapPost("/api/facilities", async (Facility facility, AppDbContext db) =>
{
    db.Facilities.Add(facility);
    await db.SaveChangesAsync();
    return Results.Created($"/api/facilities/{facility.Id}", facility);
});

app.MapGet("/api/coaches", async (AppDbContext db) => await db.Coaches.ToListAsync());
app.MapGet("/api/coaches/{id:int}", async (int id, AppDbContext db) =>
{
    var coach = await db.Coaches.FindAsync(id);
    return coach is not null ? Results.Ok(coach) : Results.NotFound();
});
app.MapPost("/api/coaches", async (Coach coach, AppDbContext db) =>
{
    db.Coaches.Add(coach);
    await db.SaveChangesAsync();
    return Results.Created($"/api/coaches/{coach.Id}", coach);
});

app.MapGet("/api/users", async (AppDbContext db) => await db.Users.ToListAsync());
app.MapPost("/api/users", async (User user, AppDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/api/users/{user.Id}", user);
});

app.MapGet("/api/sportssections", async (AppDbContext db) => await db.SportsSections.ToListAsync());
app.MapPost("/api/sportssections", async (SportsSection section, AppDbContext db) =>
{
    db.SportsSections.Add(section);
    await db.SaveChangesAsync();
    return Results.Created($"/api/sportssections/{section.Id}", section);
});

app.MapGet("/api/schedules", async (AppDbContext db) => await db.Schedules.ToListAsync());
app.MapPost("/api/schedules", async (Schedule schedule, AppDbContext db) =>
{
    db.Schedules.Add(schedule);
    await db.SaveChangesAsync();
    return Results.Created($"/api/schedules/{schedule.Id}", schedule);
});

app.MapGet("/api/enrollments", async (AppDbContext db) => await db.Enrollments.ToListAsync());
app.MapPost("/api/enrollments", async (Enrollment enrollment, AppDbContext db) =>
{
    db.Enrollments.Add(enrollment);
    await db.SaveChangesAsync();
    return Results.Created($"/api/enrollments/{enrollment.Id}", enrollment);
});
app.MapDelete("/api/enrollments/{id:int}", async (int id, AppDbContext db) =>
{
    var enrollment = await db.Enrollments.FindAsync(id);
    if (enrollment is null) return Results.NotFound();
    db.Enrollments.Remove(enrollment);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/api/news", async (AppDbContext db) => await db.News.ToListAsync());
app.MapPost("/api/news", async (News news, AppDbContext db) =>
{
    db.News.Add(news);
    await db.SaveChangesAsync();
    return Results.Created($"/api/news/{news.Id}", news);
});

app.MapGet("/api/studentprofiles", async (AppDbContext db) => await db.StudentProfiles.ToListAsync());
app.MapPost("/api/studentprofiles", async (StudentProfile profile, AppDbContext db) =>
{
    db.StudentProfiles.Add(profile);
    await db.SaveChangesAsync();
    return Results.Created($"/api/studentprofiles/{profile.Id}", profile);
});

app.Run();
