namespace ValerioProdigit.Api.Validators;

public interface IValidator
{
    
}

public interface IValidator<in T> : IValidator
{
    ValidationResult Validate(T obj);
}

public readonly record struct ValidationResult(bool Succeeded, string Error);