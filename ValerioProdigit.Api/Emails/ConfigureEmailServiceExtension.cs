using ValerioProdigit.Api.Configurations;

namespace ValerioProdigit.Api.Emails;

public static class ConfigureEmailServiceExtension
{
    public static void ConfigureEmailService(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.GetSection("Settings:SendGridSettings").Get<SendGridSettings>();

        if (settings is null)
        {
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SendGridSettings>>();
            logger.LogError("SendGridSettings is required");
            throw new Exception("SendGridSettings is required");
        }
        
        var service = new SendGridEmailService(settings.Token, settings.ServiceEmail, settings.ServiceName);
        builder.Services.AddSingleton<IEmailSender>(service);
    }
}