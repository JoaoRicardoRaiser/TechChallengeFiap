using AutoMapper;
using FluentAssertions;
using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Profiles;
using TechChallenge.UpdateContact.Domain.Entities;

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

    [Fact]
    public void ContactCreatedEventDto_To_Contact_Should_Map_Correctly()
    {
        // Arrange
        var dto = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Email = "johndoe@email.com",
            PhoneAreaCode = 47,
            Phone = "47123456789" 
        };

        var expectedContact = new Contact
        {
            Email = dto.Email,
            Phone = dto.Phone,
            PhoneAreaCode = dto.PhoneAreaCode,
            Id = dto.Id,
            Name = dto.Name
        };

        // Act
        var contact = _mapper.Map<Contact>(dto);

        // Assert
        contact.Should().NotBeNull();
        contact.Should().BeEquivalentTo(expectedContact);
    }
}