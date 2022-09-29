namespace ValerioProdigit.Api.Dtos.Account;

public class RegisterConfirmationRequest
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Token { get; set; } = "";
}