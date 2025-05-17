using System.ComponentModel.DataAnnotations;
using TechChallenge.UpdateContact.Api.Validations;

namespace TechChallenge.UpdateContact.Api.Dtos
{
    public class PutContactDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [PhoneValidation]
        public string? PhoneNumber { get; set; }

        public string PhoneAreaCode => PhoneNumber![..2];

    }
}
