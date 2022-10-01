namespace ValerioProdigit.Api.Configurations;

public sealed class PasswordSettings
{
    public bool RequireDigit { get; set; } = true;
    public int RequiredLength { get; set; } = 6;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireUppercase { get; set; } = true;
    public int RequiredUniqueChars { get; set; } = 1;
    public bool RequireNonAlphanumeric { get; set; } = true;
}