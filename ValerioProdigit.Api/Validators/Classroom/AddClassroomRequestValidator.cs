using ValerioProdigit.Api.Dtos.Classroom;
using ValerioProdigit.Api.Validators.Building;

namespace ValerioProdigit.Api.Validators.Classroom;

public sealed class AddClassroomRequestValidator : IValidator<AddClassroomRequest>
{
    private readonly BuildingValidatorSettings _buildingValidatorSettings;
    private readonly ClassroomValidatorSettings _classroomValidatorSettings;

    public AddClassroomRequestValidator(BuildingValidatorSettings buildingValidatorSettings, ClassroomValidatorSettings classroomValidatorSettings)
    {
        _buildingValidatorSettings = buildingValidatorSettings;
        _classroomValidatorSettings = classroomValidatorSettings;
    }

    public ValidationResult Validate(AddClassroomRequest obj)
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
        
        if(obj.Rows.Aggregate(0, (acc, row) => acc + row) != obj.Seats)
        {
            return new ValidationResult(false, "Capacity does not match the number of seats");
        }

        if (obj.Rows.Any(x => x == 0))
        {
            return new ValidationResult(false, "Invalid number of seats in a row");
        }
        
        if(!obj.BuildingCode.ContainsOnlyDigitOrChar())
        {
            return new ValidationResult(false, "BuildingCode is not valid");
        }

        if (!obj.ClassroomCode.ContainsOnlyDigitOrChar())
        {
            return new ValidationResult(false, "ClassroomCode is not valid");
        }

        return new ValidationResult() { Succeeded = true };
    }

    private ValidationResult LengthCheck(AddClassroomRequest obj)
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
        
        if (obj.Seats <= 0)
        {
            return new ValidationResult(false, "Invalid number of seats");
        }

        return new ValidationResult(true, "");
    }

    private ValidationResult NullCheck(AddClassroomRequest obj)
    {
        if (string.IsNullOrWhiteSpace(obj.BuildingCode))
        {
            return new ValidationResult(false, "BuildingCode is required");
        }

        if (string.IsNullOrWhiteSpace(obj.ClassroomCode))
        {
            return new ValidationResult(false, "ClassroomCode is required");
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (obj.Rows is null)
        {
            return new ValidationResult(false, "Rows is required");
        }

        return new ValidationResult(true, "");
    }
}