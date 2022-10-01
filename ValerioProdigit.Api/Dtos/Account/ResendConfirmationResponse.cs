namespace ValerioProdigit.Api.Dtos.Account;

public sealed class ResendConfirmationResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
}