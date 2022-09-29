namespace ValerioProdigit.Api.Dtos.Account;

public class ChangePasswordRequest
{
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}