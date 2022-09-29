using ValerioProdigit.Api.Configurations;

namespace ValerioProdigit.Api.Emails;

public static class ConfigureEmailServiceExtension
{
    public static void ConfigureEmailService(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.GetSection("Settings:SendGridSettings").Get<SendGridSettings>();
        var service = new SendGridEmailService(settings.Token, settings.ServiceEmail, settings.ServiceName);
        builder.Services.AddSingleton<IEmail>(service);
    }
}