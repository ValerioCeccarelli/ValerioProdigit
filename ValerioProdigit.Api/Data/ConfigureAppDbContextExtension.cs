using Microsoft.EntityFrameworkCore;

namespace ValerioProdigit.Api.Data;

public static class ConfigureAppDbContextExtension
{
    public static void ConfigureAppDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(option =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            option.UseSqlite(connectionString);
        });
    }

    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate();
    }
}