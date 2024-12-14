namespace TechChallenge.Application.Dtos;

public record PhoneDto
{
    public required string Number { get; set; }
    public int AreaCode => int.Parse(Number![..2]);
}
