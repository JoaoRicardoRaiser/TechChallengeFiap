namespace TechChallenge.UpdateContact.Application.Dtos.Events;

public class ContactUpdatedEventDto
{
    public Guid ContactId { get; set; }
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int PhoneAreaCode { get; set; }
}
