using ValerioProdigit.Api.Configurations;
using ValerioProdigit.Api.Dtos.Lesson;
using ValerioProdigit.Api.Validators.Account;

namespace ValerioProdigit.Api.Validators.Lesson;

public class GetByTeacherRequestValidator : IValidator<GetByTeacherRequest>
{
	private readonly EmailSettings _emailSettings;
	private readonly LessonValidatorSettings _lessonValidatorSettings;
	private readonly AccountValidatorSettings _accountValidatorSettings;

	public GetByTeacherRequestValidator(EmailSettings emailSettings, LessonValidatorSettings lessonValidatorSettings, AccountValidatorSettings accountValidatorSettings)
	{
		_emailSettings = emailSettings;
		_lessonValidatorSettings = lessonValidatorSettings;
		_accountValidatorSettings = accountValidatorSettings;
	}

	public ValidationResult Validate(GetByTeacherRequest obj)
	{
		var nullCheckResult = NullCheck(obj);
		if (!nullCheckResult.Succeeded)
		{
			return nullCheckResult;
		}
		
		var emailCheckResult = EmailCheck(obj);
		if (!emailCheckResult.Succeeded)
		{
			return emailCheckResult;
		}
		
		var dateCheckResult = DateCheck(obj);
		if (!dateCheckResult.Succeeded)
		{
			return dateCheckResult;
		}

		return new ValidationResult(true, "");
	}

	private ValidationResult EmailCheck(GetByTeacherRequest obj)
	{
		if (obj.TeacherEmail.Length > _accountValidatorSettings.MaxEmailLength)
		{
			return new ValidationResult()
			{
				Succeeded = false,
				Error = "Email too long"
			};
		}
		
		if (!_emailSettings.EmailRegex.IsMatch(obj.TeacherEmail))
		{
			return new ValidationResult(false, "Invalid email");
		}

		var domain = obj.TeacherEmail.Split("@")[1];
		if(!_emailSettings.AllowedNonStudentDomains.Contains(domain))
		{
			return new ValidationResult(false, "Invalid email domain");
		}
		
		return new ValidationResult(true, "");
	}

	private ValidationResult NullCheck(GetByTeacherRequest obj)
	{
		if(string.IsNullOrWhiteSpace(obj.TeacherEmail))
		{
			return new ValidationResult(false, "Teacher email is required");
		}
		if(string.IsNullOrWhiteSpace(obj.Date))
		{
			return new ValidationResult(false, "Date is required");
		}
		
		return new ValidationResult(true, "");
	}

	private ValidationResult DateCheck(GetByTeacherRequest obj)
	{
		if(!_lessonValidatorSettings.DateRegex.IsMatch(obj.Date))
		{
			return new ValidationResult(false, "Date format is not valid");
		}

		var isValid = DateTime.TryParse(obj.Date, out var date);
		if(!isValid)
		{
			return new ValidationResult(false, "Date format is not valid");
		}
		
		if(date < DateTime.Today)
		{
			return new ValidationResult(false, "Date must be in the future");
		}
		
		return new ValidationResult(true, "");
	}
}