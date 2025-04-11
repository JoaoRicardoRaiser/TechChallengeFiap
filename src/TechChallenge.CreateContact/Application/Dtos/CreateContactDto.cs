namespace TechChallenge.CreateContact.Application.Dtos;

public record CreateContactDto
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public PhoneDto Phone { get; set; } = default!;
}