using ValerioProdigit.Api.Dtos.Lesson;
using ValerioProdigit.Api.Validators.Building;
using ValerioProdigit.Api.Validators.Classroom;

namespace ValerioProdigit.Api.Validators.Lesson;

public class GetByClassroomRequestValidator : IValidator<GetByClassroomRequest>
{
	private readonly BuildingValidatorSettings _buildingValidatorSettings;
	private readonly ClassroomValidatorSettings _classroomValidatorSettings;
	private readonly LessonValidatorSettings _lessonValidatorSettings;

	public GetByClassroomRequestValidator(BuildingValidatorSettings buildingValidatorSettings, ClassroomValidatorSettings classroomValidatorSettings, LessonValidatorSettings lessonValidatorSettings)
	{
		_buildingValidatorSettings = buildingValidatorSettings;
		_classroomValidatorSettings = classroomValidatorSettings;
		_lessonValidatorSettings = lessonValidatorSettings;
	}

	public ValidationResult Validate(GetByClassroomRequest obj)
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

		var dateCheckResult = DateCheck(obj);
		if(!dateCheckResult.Succeeded)
		{
			return dateCheckResult;
		}
		
		if(!obj.BuildingCode.ContainsOnlyDigitOrChar())
		{
			return new ValidationResult(false, "BuildingCode is not valid");
		}

		if (!obj.ClassroomCode.ContainsOnlyDigitOrChar())
		{
			return new ValidationResult(false, "ClassroomCode is not valid");
		}
		
		return new ValidationResult(true, "");
	}

	private ValidationResult DateCheck(GetByClassroomRequest obj)
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

	private ValidationResult LengthCheck(GetByClassroomRequest obj)
	{
		if(obj.BuildingCode.Length > _buildingValidatorSettings.MaxCodeLength)
		{
			return new ValidationResult()
			{
				Succeeded = false,
				Error = "BuildingCode too long"
			};
		}
        
		if(obj.ClassroomCode.Length > _classroomValidatorSettings.MaxCodeLength)
		{
			return new ValidationResult()
			{
				Succeeded = false,
				Error = "ClassroomCode too long"
			};
		}

		return new ValidationResult(true, "");
	}

	private ValidationResult NullCheck(GetByClassroomRequest obj)
	{
		if (string.IsNullOrWhiteSpace(obj.BuildingCode))
		{
			return new ValidationResult(false, "Building code is required");
		}
		if(string.IsNullOrWhiteSpace(obj.ClassroomCode))
		{
			return new ValidationResult(false, "Classroom code is required");
		}
		if(string.IsNullOrWhiteSpace(obj.Date))
		{
			return new ValidationResult(false, "Date is required");
		}
		
		return new ValidationResult(true, "");
	}
}