using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Api.Validation;

public class PhoneValidation: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(value?.ToString()) || value.ToString()!.Length != 11)
            return new ValidationResult("Phone format is invalid. Format accepted is: 99123456789 (DDD+number, without spaces and ())");

        return ValidationResult.Success;
    }
}
