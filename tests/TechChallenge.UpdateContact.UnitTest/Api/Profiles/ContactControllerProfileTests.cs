using AutoMapper;
using FluentAssertions;
using TechChallenge.UpdateContact.Api.Dtos;
using TechChallenge.UpdateContact.Api.Profiles;
using TechChallenge.UpdateContact.Application.Dtos;

namespace TechChallenge.UpdateContact.UnitTest.Api.Profiles;

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
