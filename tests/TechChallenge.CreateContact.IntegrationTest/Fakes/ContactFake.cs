using TechChallenge.CreateContact.Api.Dtos;
using TechChallenge.CreateContact.Domain.Entities;

namespace TechChallenge.CreateContact.IntegrationTest.Fakes;

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

    public static PostContactDto NewPostDto()
        => new()
        {
            Name = "King Green",
            Email = "kinggreen@mail.com",
            PhoneNumber = "47123459876"
        };
}
