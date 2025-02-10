using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.UnitTest.Fixtures;
public static class ContactFake
{

    public static Contact New(string name)
        => new() 
        {
            Id = Guid.NewGuid(),
            Email = "johndoe@email.com",
            Name = name,
            Phone = "47123456789",
            PhoneAreaCode = 47,
            PhoneArea = new PhoneArea { Code = 47, Region = "Region" }
        };
}
