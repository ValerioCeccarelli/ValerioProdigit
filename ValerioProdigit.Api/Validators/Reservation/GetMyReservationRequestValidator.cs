using ValerioProdigit.Api.Dtos.Reservation;
using ValerioProdigit.Api.Validators.Lesson;

namespace ValerioProdigit.Api.Validators.Reservation;

public class GetMyReservationRequestValidator : IValidator<GetMyReservationRequest>
{
	private readonly LessonValidatorSettings _lessonValidatorSettings;

	public GetMyReservationRequestValidator(LessonValidatorSettings lessonValidatorSettings)
	{
		_lessonValidatorSettings = lessonValidatorSettings;
	}

	public ValidationResult Validate(GetMyReservationRequest obj)
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