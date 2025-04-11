using Newtonsoft.Json;

namespace TechChallenge.CreateContact.Domain.Entities;

public class Contact: EntityBase 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int PhoneAreaCode { get; set; }

    [JsonIgnore]
    public virtual PhoneArea PhoneArea { get; set; } = default!;
}
