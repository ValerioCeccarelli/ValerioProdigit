using ValerioProdigit.Api.Configurations;
using ValerioProdigit.Api.Validators.Account;
using ValerioProdigit.Api.Validators.Building;
using ValerioProdigit.Api.Validators.Classroom;
using ValerioProdigit.Api.Validators.Lesson;

namespace ValerioProdigit.Api.Validators;

public static class ValidatorExtension
{
    private static void ConfigureEmailSettings(this WebApplicationBuilder builder)
    {
        var emailSetting = builder.Configuration
            .GetSection("Settings:EmailSettings")
            .Get<EmailSettings>();

        emailSetting.AllowedDomains = new HashSet<string>();
        emailSetting.AllowedNonStudentDomains = new HashSet<string>();
        foreach (var allowedAdminDomain in emailSetting.AllowedAdminDomains)
        {
            emailSetting.AllowedDomains.Add(allowedAdminDomain);
            emailSetting.AllowedNonStudentDomains.Add(allowedAdminDomain);
        }
        foreach (var allowedProfDomain in emailSetting.AllowedTeacherDomains)
        {
            emailSetting.AllowedDomains.Add(allowedProfDomain);
            emailSetting.AllowedNonStudentDomains.Add(allowedProfDomain);
        }
        foreach (var allowedStudentDomain in emailSetting.AllowedStudentsDomains)
        {
            emailSetting.AllowedDomains.Add(allowedStudentDomain);
        }

        builder.Services.AddSingleton(_ => emailSetting);
    }

    private static void ConfigureSettings(this WebApplicationBuilder builder)
    {
        builder.ConfigureEmailSettings();

        var validatorSection = builder.Configuration.GetSection("Settings:ValidatorsSettings");
        
        var accountValidatorSettings = validatorSection.GetSection("AccountValidatorSettings").Get<AccountValidatorSettings>();
        var buildingValidatorSettings = validatorSection.GetSection("BuildingValidatorSettings").Get<BuildingValidatorSettings>();
        var classroomValidatorSettings = validatorSection.GetSection("ClassroomValidatorSettings").Get<ClassroomValidatorSettings>();
        var lessonValidatorSettings = validatorSection.GetSection("LessonValidatorSettings").Get<LessonValidatorSettings>();

        if (accountValidatorSettings is null)
            throw new Exception("'Settings:ValidatorsSettings:AccountValidatorSettings' is required in appsettings.json");
        if (buildingValidatorSettings is null)
            throw new Exception("'Settings:ValidatorsSettings:BuildingValidatorSettings' is required in appsettings.json");
        if (classroomValidatorSettings is null)
            throw new Exception("'Settings:ValidatorsSettings:ClassroomValidatorSettings' is required in appsettings.json");
        if (lessonValidatorSettings is null)
            throw new Exception("'Settings:ValidatorsSettings:LessonValidatorSettings' is required in appsettings.json");

        builder.Services.AddSingleton(_ => accountValidatorSettings);
        builder.Services.AddSingleton(_ => buildingValidatorSettings);
        builder.Services.AddSingleton(_ => classroomValidatorSettings);
        builder.Services.AddSingleton(_ => lessonValidatorSettings);
    }
    
    public static void ConfigureValidators(this WebApplicationBuilder builder)
    {
        builder.ConfigureSettings();

        var validatorsTuple = typeof(ValidatorExtension).Assembly.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IValidator)))
            .Where(x => !x.IsInterface && !x.IsAbstract)
            .Select(GetIValidator)
            .Where(x => x.serviceType is not null)
            .ToArray();

        foreach (var tuple in validatorsTuple)
        {
            if (validatorsTuple.Any(x => x != tuple && tuple.serviceType == x.serviceType))
            {
                var arg = tuple.serviceType!.GetGenericArguments()[0];
                throw new Exception($"Too many Validators are referred to {arg.FullName}, only one is allowed");
            }
            
            builder.Services.AddSingleton(tuple.serviceType!, tuple.implementationType);
        }
    }

    private static (Type? serviceType, Type implementationType) GetIValidator(Type type) 
    {
        var interfaces = type.GetInterfaces();
        foreach (var possibleValidatorInterface in interfaces)
        {
            try
            {
                var ris = possibleValidatorInterface.GetGenericTypeDefinition() == typeof(IValidator<>);
                if (ris)
                {
                    return (possibleValidatorInterface, type);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return (null, type);
    }
    
    public static bool ContainsOnlyDigitOrChar(this string str)
    {
        return str.All(x => char.IsDigit(x) || char.IsLetter(x));
    }
}