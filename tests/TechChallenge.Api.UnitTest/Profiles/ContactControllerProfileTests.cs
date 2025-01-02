using AutoMapper;
using FluentAssertions;
using TechChallenge.Api.Dtos;
using TechChallenge.Api.Profiles;
using TechChallenge.Application.Dtos;

namespace TechChallenge.Api.UnitTest.Profiles;
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

    [Fact]
    public void PutContactDto_To_UpdateContactDto_Should_Map_Correctly()
    {
        // Assert
        var postContactDto = new PutContactDto
        {
            Email = "johndoe@email.com",
            PhoneNumber = "47123456789"
        };

        var expectedCreateContactDto = new UpdateContactDto
        {
            Email = postContactDto.Email,
            Phone = new PhoneDto { Number = postContactDto.PhoneNumber }
        };

        // Act
        var createContactDto = _mapper.Map<UpdateContactDto>(postContactDto);

        // Assert
        createContactDto.Should().NotBeNull();
        createContactDto.Should().BeEquivalentTo(expectedCreateContactDto);
    }
}
