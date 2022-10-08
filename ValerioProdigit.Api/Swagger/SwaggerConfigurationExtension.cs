using System.Net;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ValerioProdigit.Api.Swagger;

public static class SwaggerConfigurationExtension
{
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Exaple: Bearer jwttoken123"
        };

        var securityRequirement = new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    }
                },
                Array.Empty<string>()
            }
        };

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.ExampleFilters();
            option.AddSwaggerDoc(builder);
            option.AddSecurityDefinition("Bearer", securityScheme);
            option.AddSecurityRequirement(securityRequirement);

            option.EnableAnnotations();
			
            // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
    }

    private static void AddSwaggerDoc(this SwaggerGenOptions option, WebApplicationBuilder builder)
    {
        SwaggerSettings? swaggerSettings = builder.Configuration.GetSection("Swaggerz").Get<SwaggerSettings>();
        
        var contactInfo = new OpenApiContact()
        {
            Name = swaggerSettings.Contact.Name,
            Email = swaggerSettings.Contact.Email,
            Url = new Uri(swaggerSettings.Contact.Url)
        };

        var license = new OpenApiLicense()
        {
            Name = swaggerSettings.License.Name,
            Url = new Uri(swaggerSettings.License.Url)
        };

        var info = new OpenApiInfo()
        {
            Version = swaggerSettings.Info.Version,
            Title = swaggerSettings.Info.Title,
            Description = swaggerSettings.Info.Description,
            Contact = contactInfo,
            License = license,
        };

        if (swaggerSettings is null)
        {
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
            logger.LogWarning("Swagger settings not found");
            return;
        }

        if (!swaggerSettings.IsValid(out var error))
        {
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
            logger.LogWarning("Swagger settings not valid: {error}", error);
            return;
        }
        
        option.SwaggerDoc(swaggerSettings.Info.Version.ToLower(), info);
    }
	
    public static void UseSwaggerEndpoint(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    public static RouteHandlerBuilder WithDocumentation(this RouteHandlerBuilder builder, string summary,
        string description) => builder.WithMetadata(new SwaggerOperationAttribute(summary, description));

    public static RouteHandlerBuilder WithResponseDocumentation<TData>(this RouteHandlerBuilder builder,
        HttpStatusCode statusCode, string description) =>
        builder.WithMetadata(new SwaggerResponseAttribute((int)statusCode, description, typeof(TData)));
    
    public static RouteHandlerBuilder WithResponseDocumentation(this RouteHandlerBuilder builder,
        HttpStatusCode statusCode, string description) =>
        builder.WithMetadata(new SwaggerResponseAttribute((int)statusCode, description, typeof(void)));
}