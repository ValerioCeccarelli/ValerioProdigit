using ValerioProdigit.Api.Configurations;

namespace ValerioProdigit.Api.Emails;

public static class ConfigureEmailServiceExtension
{
    public static void ConfigureEmailService(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.GetSection("Settings:SendGridSettings").Get<SendGridSettings>();

        if (settings is not null && settings.IsActive)
        {
            var service = new SendGridEmailService(settings.Token, settings.ServiceEmail, settings.ServiceName);
            builder.Services.AddSingleton<IEmailSender>(service);
        }
        else
        {
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SendGridSettings>>();
            logger.LogWarning("SendGrid is not configured. Using EmptyEmailSender instead.");
            
            builder.Services.AddSingleton<IEmailSender, EmptyEmailService>();
        }
    }
}