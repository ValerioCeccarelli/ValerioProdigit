using ValerioProdigit.Api.Configurations;
using ValerioProdigit.Api.Dtos.Account;

namespace ValerioProdigit.Api.Validators.Account;

public sealed class RegisterRequestValidator : IValidator<RegisterRequest>
{
    private readonly EmailSettings _emailSettings;
    private readonly AccountValidatorSettings _accountValidatorSettings;

    public RegisterRequestValidator(EmailSettings emailSettings, AccountValidatorSettings accountValidatorSettings)
    {
        _emailSettings = emailSettings;
        _accountValidatorSettings = accountValidatorSettings;
    }

    public ValidationResult Validate(RegisterRequest obj)
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
        
        var formatCheckResult = FormatCheck(obj);
        if (!formatCheckResult.Succeeded)
        {
            return formatCheckResult;
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

    private ValidationResult NullCheck(RegisterRequest obj)
    {
        if(string.IsNullOrWhiteSpace(obj.Name))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Name required"
            };
        }
        if(string.IsNullOrWhiteSpace(obj.Surname))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Surname required"
            };
        }
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

    private ValidationResult LengthCheck(RegisterRequest obj)
    {
        if (obj.Name.Length > _accountValidatorSettings.MaxNameLength)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Name too long"
            };
        }
        if (obj.Surname.Length > _accountValidatorSettings.MaxSurnameLength)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Surname too long"
            };
        }
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

    private ValidationResult FormatCheck(RegisterRequest obj)
    {
        if (!_accountValidatorSettings.NameRegex.IsMatch(obj.Name))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Invalid name"
            };
        }
        
        if (!_accountValidatorSettings.NameRegex.IsMatch(obj.Surname))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Invalid surname"
            };
        }
        
        if (!_emailSettings.EmailRegex.IsMatch(obj.Email))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Invalid email"
            };
        }
        
        return new ValidationResult() { Succeeded = true };
    }
}