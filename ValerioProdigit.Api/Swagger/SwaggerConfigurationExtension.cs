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
        SwaggerSettings? swaggerSettings = builder.Configuration.GetSection("Swagger").Get<SwaggerSettings>();
        
        if (swaggerSettings is null)
        {
            using var scope = builder.Services.BuildServiceProvider().CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SwaggerSettings>>();
            logger.LogWarning("Swagger settings not found");
            return;
        }

        var info = new OpenApiInfo();

        if (swaggerSettings.Contact is not null)
        {
            info.Contact = new OpenApiContact()
            {
                Name = swaggerSettings.Contact.Name,
                Email = swaggerSettings.Contact.Email,
                Url = new Uri(swaggerSettings.Contact.Url)
            };
        }

        if (swaggerSettings.License is not null)
        {
            info.License = new OpenApiLicense()
            {
                Name = swaggerSettings.License.Name,
                Url = new Uri(swaggerSettings.License.Url)
            };
        }

        info.Version = swaggerSettings.Info?.Version;
        info.Title = swaggerSettings.Info?.Title;
        info.Description = swaggerSettings.Info?.Description;

        var version = swaggerSettings.Info?.Version.ToLower() ?? "v1";
        option.SwaggerDoc(version, info);
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