using AutoMapper;
using FluentAssertions;
using TechChallenge.CreateContact.Api.Dtos;
using TechChallenge.CreateContact.Api.Profiles;
using TechChallenge.CreateContact.Application.Dtos;

namespace TechChallenge.CreateContact.UnitTest.Api.Profiles;

public class ContactControllerProfileTests
{
    private readonly IMapper _mapper;

    public ContactControllerProfileTests()
    {
        _mapper = new MapperConfiguration(x => x.AddProfile(typeof(ContactControllerProfile)))
            .CreateMapper();
    }

    [Fact]
    public void PostContactDto_To_CreateContactDto_Should_Map_Correctly()
    {
        // Assert
        var postContactDto = new PostContactDto
        {
            Email = "johndoe@email.com",
            Name = "John Doe",
            PhoneNumber = "47123456789"
        };

        var expectedCreateContactDto = new CreateContactDto
        {
            Email = postContactDto.Email,
            Name = postContactDto.Name,
            Phone = new PhoneDto { Number = postContactDto.PhoneNumber }
        };

        // Act
        var createContactDto = _mapper.Map<CreateContactDto>(postContactDto);

        // Assert
        createContactDto.Should().NotBeNull();
        createContactDto.Should().BeEquivalentTo(expectedCreateContactDto);
    }
}
