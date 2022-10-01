namespace ValerioProdigit.Api.Dtos.Account;

public sealed class RegisterResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";
}