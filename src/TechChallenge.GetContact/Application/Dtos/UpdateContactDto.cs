namespace TechChallenge.GetContact.Application.Dtos
{
    public class UpdateContactDto
    {
        public Guid ContactId { get; set; }
        public required string Email { get; set; }
        public PhoneDto Phone { get; set; } = default!;
    }
}
