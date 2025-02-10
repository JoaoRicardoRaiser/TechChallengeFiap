using System.ComponentModel.DataAnnotations;
using TechChallenge.Api.Validation;

namespace TechChallenge.Api.Dtos;

public record PutContactDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [PhoneValidation]
    public string? PhoneNumber { get; set; }

    public string PhoneAreaCode => PhoneNumber![..2];
}
