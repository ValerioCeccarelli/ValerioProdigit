namespace ValerioProdigit.Api.Configurations;

public class JwtSettings
{
	public string Secret { get; set; } = null!;
	public TimeSpan ExpiryTime { get; set; }
	public string Issuer { get; set; } = null!;
	public string Audience { get; set; } = null!;
}