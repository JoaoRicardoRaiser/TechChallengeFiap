using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Api.Dtos;

public record PutContactDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }

    public string PhoneAreaCode => PhoneNumber![..2];
}
