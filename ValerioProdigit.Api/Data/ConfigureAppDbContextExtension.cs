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
}