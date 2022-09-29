using Microsoft.AspNetCore.Identity;
using ValerioProdigit.Api.Configurations;

namespace ValerioProdigit.Api.Auth;

public static class CustomAuthorizationExtension
{
    public static void ConfigureCustomAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddAdminPolicy();
            options.AddNonStudentPolicy();
        });
    }

    public static void ConfigurePasswordOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<IdentityOptions>(options =>
        {
            var passwordSettings = builder.Configuration
                .GetSection("Settings:PasswordSettings")
                .Get<PasswordSettings>();
            options.Password.RequireDigit = passwordSettings.RequireDigit;
            options.Password.RequiredLength = passwordSettings.RequiredLength;
            options.Password.RequireLowercase = passwordSettings.RequireLowercase;
            options.Password.RequireUppercase = passwordSettings.RequireUppercase;
            options.Password.RequiredUniqueChars = passwordSettings.RequiredUniqueChars;
            options.Password.RequireNonAlphanumeric = passwordSettings.RequireNonAlphanumeric;
        });
    }
}