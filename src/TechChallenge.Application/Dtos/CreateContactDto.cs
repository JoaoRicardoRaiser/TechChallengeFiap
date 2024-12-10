namespace TechChallenge.Application.Dtos;

public class CreateContactDto
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public PhoneDto PhoneNumber { get; set; } = default!;
}

public class PhoneDto
{
    public required string Number { get; set; }
    public int PhoneAreaCode => int.Parse(Number![..2]);
}
