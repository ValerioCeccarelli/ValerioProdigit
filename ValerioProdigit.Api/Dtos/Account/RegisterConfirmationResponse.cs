namespace ValerioProdigit.Api.Dtos.Account;

public class RegisterConfirmationResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";
    public string Token { get; set; } = "";
}