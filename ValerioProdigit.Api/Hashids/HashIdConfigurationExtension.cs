using HashidsNet;
using ValerioProdigit.Api.Configurations;

namespace ValerioProdigit.Api.Hashids;

public static class HashIdConfigurationExtension
{
	public static void ConfigureHashId(this WebApplicationBuilder builder)
	{
		var hashId = builder.Configuration
			.GetSection("Settings:HashidSettings")
			.Get<HashidSettings>();
		
		builder.Services.AddSingleton<IHashids>(_ => new HashidsNet.Hashids(hashId.Salt, hashId.MinLength));
	}
}