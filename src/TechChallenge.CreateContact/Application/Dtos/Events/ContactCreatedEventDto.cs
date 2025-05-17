namespace TechChallenge.CreateContact.Application.Dtos.Events;

public class ContactCreatedEventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int PhoneAreaCode { get; set; }
}
