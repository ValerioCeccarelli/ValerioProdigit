namespace ValerioProdigit.Api.Configurations;

public sealed class SendGridSettings
{
    public bool IsActive { get; set; }
    public string Token { get; set; } = default!;
    public string ServiceEmail { get; set; } = default!;
    public string ServiceName { get; set; } = default!;
}