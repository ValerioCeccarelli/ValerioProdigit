using ValerioProdigit.Api.Dtos.Account;

namespace ValerioProdigit.Api.Validators.Account;

public class ChangePasswordRequestValidator : IValidator<ChangePasswordRequest>
{
    public ValidationResult Validate(ChangePasswordRequest obj)
    {
        if (string.IsNullOrWhiteSpace(obj.OldPassword))
        {
            return new ValidationResult()
            {
                Error = "OldPassword required"
            };
        }
        if (string.IsNullOrWhiteSpace(obj.NewPassword))
        {
            return new ValidationResult()
            {
                Error = "NewPassword required"
            };
        }
        
        if (obj.OldPassword.Length > 50)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "OldPassword too long"
            };
        }
        if (obj.NewPassword.Length > 50)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "NewPassword too long"
            };
        }

        return new ValidationResult() { Succeeded = true };
    }
}