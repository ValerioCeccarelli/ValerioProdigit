using ValerioProdigit.Api.Dtos.Building;

namespace ValerioProdigit.Api.Validators.Building;

public sealed class AddBuildingRequestValidator : IValidator<AddBuildingRequest>
{
    private readonly BuildingValidatorSettings _buildingSettings;

    public AddBuildingRequestValidator(BuildingValidatorSettings buildingSettings)
    {
        _buildingSettings = buildingSettings;
    }

    public ValidationResult Validate(AddBuildingRequest obj)
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
        
        if(!obj.Code.ContainsOnlyDigitOrChar())
        {
            return new ValidationResult(false, "Code is not valid");
        }

        return new ValidationResult() { Succeeded = true };
    }

    private ValidationResult NullCheck(AddBuildingRequest obj)
    {
        if (string.IsNullOrWhiteSpace(obj.Name))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Name required"
            };
        }
        if (string.IsNullOrWhiteSpace(obj.Code))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Code required"
            };
        }
        if (string.IsNullOrWhiteSpace(obj.Address))
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Address required"
            };
        }
        
        return new ValidationResult() { Succeeded = true };
    }
    
    private ValidationResult LengthCheck(AddBuildingRequest obj)
    {
        if (obj.Name.Length > _buildingSettings.MaxNameLength)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Name too long"
            };
        }
        if (obj.Code.Length > _buildingSettings.MaxCodeLength)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Code too long"
            };
        }
        if (obj.Address.Length > _buildingSettings.MaxAddressLength)
        {
            return new ValidationResult()
            {
                Succeeded = false,
                Error = "Address too long"
            };
        }
        
        return new ValidationResult() { Succeeded = true };
    }
}