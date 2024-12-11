namespace TechChallenge.Application.Dtos;

public record UpdateContactDto
{
    public Guid ContactId { get; set; }
    public required string Email { get; set; }
    public PhoneDto Phone { get; set; } = default!;
}
