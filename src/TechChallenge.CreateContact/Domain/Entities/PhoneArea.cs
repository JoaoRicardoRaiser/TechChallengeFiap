namespace TechChallenge.CreateContact.Domain.Entities;

public class PhoneArea: EntityBase
{
    public int Code {  get; set; }
    public string Region { get; set; } = default!;
}
