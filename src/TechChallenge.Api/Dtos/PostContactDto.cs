using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Api.Dtos;

public class PostContactDto
{
    [Required]
    [Length(3, int.MaxValue)]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public string? PhoneAreaCode => PhoneNumber?.Substring(0, 2);
}
