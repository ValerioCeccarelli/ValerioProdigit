namespace ValerioProdigit.Api.Dtos.Account;

public class ResendConfirmationRequest
{
	public string Email { get; set; } = default!;
	public string Password { get; set; } = default!;
}