namespace ValerioProdigit.Api.Dtos.Account;

public class RegisterRequest
{
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}