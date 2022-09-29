using ValerioProdigit.Api.Configurations;
using ValerioProdigit.Api.Dtos.Account;

namespace ValerioProdigit.Api.Validators.Account;

public class LoginRequestValidator : IValidator<LoginRequest>
{
    private readonly EmailSettings _emailSettings;
    private readonly AccountValidatorSettings _accountValidatorSettings;

    public LoginRequestValidator(EmailSettings emailSettings, AccountValidatorSettings accountValidatorSettings)
    {
        _emailSettings = emailSettings;
        _accountValidatorSettings = accountValidatorSettings;
    }

    public ValidationResult Validate(LoginRequest obj)
    {
        var nullCheckResult = NullCheck(obj);
        if (!nullCheckResult.Succeeded)
        {
            return nullCheckResult;
        }
        
        var lengthCheckResult = LengthCheck(obj);
        if (!lengthCheckResult.Succeeded)
        {
            return lengthCheckResult;
        }
        
        if (!_emailSettings.EmailRegex.IsMatch(obj.Email))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Invalid email"
            };
        }
        
        var domain = obj.Email.Split('@')[1];
        if (!_emailSettings.AllowedDomains.Contains(domain))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Invalid email"
            };
        }

        //Password check is performed by the .net library
        return new ValidationResult() { Succeeded = true };
    }

    private ValidationResult NullCheck(LoginRequest obj)
    {
        if(string.IsNullOrWhiteSpace(obj.Email))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Email required"
            };
        }
        if(string.IsNullOrWhiteSpace(obj.Password))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Password required"
            };
        }

        return new ValidationResult() { Succeeded = true };
    }

    private ValidationResult LengthCheck(LoginRequest obj)
    {
        if (obj.Email.Length > _accountValidatorSettings.MaxEmailLength)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Email too long"
            };
        }
        if (obj.Password.Length > _accountValidatorSettings.MaxPasswordLength)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Password too long"
            };
        }
        
        return new ValidationResult() { Succeeded = true };
    }
}