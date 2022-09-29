using System.Net;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ValerioProdigit.Api.Swagger;

public static class SwaggerConfigurationExtension
{
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        var swaggerSection = builder.Configuration.GetSection("Swagger");

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

        var contactInfo = new OpenApiContact()
        {
            Name = swaggerSection["Contact:Name"],
            Email = swaggerSection["Contact:Email"],
            Url = new Uri(swaggerSection["Contact:Url"])
        };

        var license = new OpenApiLicense()
        {
            Name = swaggerSection["License:Name"],
            Url = new Uri(swaggerSection["License:Url"])
        };

        var info = new OpenApiInfo()
        {
            Version = swaggerSection["Info:Version"],
            Title = swaggerSection["Info:Title"],
            Description = swaggerSection["Info:Description"],
            Contact = contactInfo,
            License = license,
        };
		
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.ExampleFilters();
            option.SwaggerDoc(swaggerSection["Info:Version"].ToLower(), info);
            option.AddSecurityDefinition("Bearer", securityScheme);
            option.AddSecurityRequirement(securityRequirement);

            option.EnableAnnotations();
			
            // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
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