
using TechChallenge.DeleteContact.Domain.Entities;

namespace TechChallenge.DeleteContact.IntegrationTest.Fakes;

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
}
