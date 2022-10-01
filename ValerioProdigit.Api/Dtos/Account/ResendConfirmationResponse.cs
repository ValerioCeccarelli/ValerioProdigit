namespace ValerioProdigit.Api.Dtos.Account;

public class ResendConfirmationResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
}