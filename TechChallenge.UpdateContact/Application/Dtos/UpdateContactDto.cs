using TechChallenge.CreateContact.Application.Dtos;

namespace TechChallenge.UpdateContact.Application.Dtos
{
    public class UpdateContactDto
    {
        public Guid ContactId { get; set; }
        public required string Email { get; set; }
        public PhoneDto Phone { get; set; } = default!;
    }
}
