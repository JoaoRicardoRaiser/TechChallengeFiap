namespace TechChallenge.GetContact.Application.Dtos
{
    public class CreateContactDto
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public PhoneDto Phone { get; set; } = default!;
    }
}
