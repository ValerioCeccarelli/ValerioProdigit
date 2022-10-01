namespace ValerioProdigit.Api.Dtos.Account;

public sealed class ResendConfirmationRequest
{
	public string Email { get; set; } = default!;
	public string Password { get; set; } = default!;
}