using AutoMapper;
using FluentAssertions;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Profiles;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.UnitTest.Profiles;

public class ContactProfileTests
{
    private readonly IMapper _mapper;

    public ContactProfileTests()
    {
        _mapper = new MapperConfiguration(x => x.AddProfile(typeof(ContactProfile)))
            .CreateMapper();
    }

    [Fact]
    public void CreateContactDto_To_Contact_Should_Map_Correctly()
    {
        // Arrange
        var createContactDto = new CreateContactDto
        {
            Name = "John Doe",
            Email = "johndoe@email.com",
            Phone = new PhoneDto { Number = "47123456789" }
        };

        var expectedContact = new Contact
        {
            Name = createContactDto.Name,
            Email = createContactDto.Email,
            Phone = createContactDto.Phone.Number,
            PhoneAreaCode = createContactDto.Phone.AreaCode
        };

        // Act
        var contact = _mapper.Map<Contact>(createContactDto);

        // Assert
        contact.Should().NotBeNull();
        contact.Should().BeEquivalentTo(expectedContact);
    }

    [Fact]
    public void UpdateContactDto_To_Contact_Should_Map_Correctly()
    {
        // Arrange
        var createContactDto = new UpdateContactDto
        {
            Email = "johndoe@email.com",
            Phone = new PhoneDto { Number = "47123456789" }
        };

        var expectedContact = new Contact
        {
            Email = createContactDto.Email,
            Phone = createContactDto.Phone.Number,
            PhoneAreaCode = createContactDto.Phone.AreaCode
        };

        // Act
        var contact = _mapper.Map<Contact>(createContactDto);

        // Assert
        contact.Should().NotBeNull();
        contact.Should().BeEquivalentTo(expectedContact);
    }
}
