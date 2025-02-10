using TechChallenge.Api.Dtos;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Api.IntegrationTest.Fakes;
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

    public static PutContactDto NewPutDto()
        => new()
        {
            Email = "drewdoberupdated@mail.com",
            PhoneNumber = "71985231457"
        };

}
