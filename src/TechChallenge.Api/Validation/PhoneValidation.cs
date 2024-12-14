using System.ComponentModel.DataAnnotations;
using TechChallenge.Application.Interfaces;

namespace TechChallenge.Api.Validation;

public class PhoneValidation: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var phoneAreaCache = validationContext.GetRequiredService<IPhoneAreaCache>();

        if (string.IsNullOrWhiteSpace(value?.ToString()) || value.ToString()!.Length != 11)
            return new ValidationResult("Phone format is invalid. Format accepted is: 99123456789 (DDD+number, without spaces and ())");

        var phoneArea = int.Parse(value.ToString()![..2]);                             
        
        if(!phoneAreaCache.Exists(phoneArea))
            return new ValidationResult($"Phone area code not exists. Code: {phoneArea}");

        return ValidationResult.Success;
    }
}
