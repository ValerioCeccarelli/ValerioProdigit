namespace ValerioProdigit.Api.Dtos.Account;

public sealed class LoginResponse
{
    public bool Success => Error.Length == 0;
    public string Token { get; set; } = "";
    public string Error { get; set; } = "";
}