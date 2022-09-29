using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ValerioProdigit.Api.Auth.Services;
using ValerioProdigit.Api.Configurations;

namespace ValerioProdigit.Api.Auth;

public static class JwtConfigurationExtension
{
	public static void ConfigureJwtAuthentication(this WebApplicationBuilder builder)
	{
		var jwtSettings = builder.Configuration
			.GetSection("Settings:JwtSettings")
			.Get<JwtSettings>();
		builder.Services.AddSingleton(jwtSettings);

		var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtSettings.Issuer,
			
			ValidateAudience = true,
			ValidAudience = jwtSettings.Audience,
			
			IssuerSigningKey = new SymmetricSecurityKey(key),
			ValidateLifetime = true,
			RequireExpirationTime = true,
			ValidateIssuerSigningKey = true,
			ClockSkew = TimeSpan.Zero
		};

		builder.Services.AddSingleton(typeof(TokenValidationParameters),tokenValidationParameters);
		
		builder.Services.AddAuthentication(options => {
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer (options =>
		{
			options.TokenValidationParameters = tokenValidationParameters;
		});

		builder.Services.AddScoped<JwtGenerator>();
	}
	
	
}