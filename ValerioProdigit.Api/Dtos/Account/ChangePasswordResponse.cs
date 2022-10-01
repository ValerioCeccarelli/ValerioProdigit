namespace ValerioProdigit.Api.Dtos.Account;

public sealed class ChangePasswordResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";
}