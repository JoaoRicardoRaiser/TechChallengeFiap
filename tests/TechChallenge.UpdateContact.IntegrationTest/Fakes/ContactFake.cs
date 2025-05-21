using TechChallenge.UpdateContact.Api.Dtos;
using TechChallenge.UpdateContact.Domain.Entities;

namespace TechChallenge.UpdateContact.IntegrationTest.Fakes;

public static class ContactFake
{
    public static Contact New(string name)
        => new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = $"{name.Replace(" ", "").ToLower()}@mail.com",
            Phone = "11987651234",
            PhoneAreaCode = 11
        };

    public static PutContactDto NewPutDto()
        => new()
        {
            Email = "drewdoberupdated@mail.com",
            PhoneNumber = "71985231457"
        };
}
