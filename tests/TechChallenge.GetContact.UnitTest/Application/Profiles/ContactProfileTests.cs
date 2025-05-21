using AutoMapper;
using FluentAssertions;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Profiles;
using TechChallenge.GetContact.Application.UnitTest.Fixtures;
using TechChallenge.GetContact.Domain.Entities;

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
    public void ContactUpdatedEventDto_To_Contact_Should_Map_Correctly()
    {
        // Arrange
        var dto = new ContactUpdatedEventDto
        {
            ContactId = Guid.NewGuid(),
            Email = "test@mail.com",
            Name = "Test",
            Phone = "47965232104",
            PhoneAreaCode = 47
        };

        var contact = ContactFake.New("John");

        var expectedContact = new Contact
        {
            Id = contact.Id,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            PhoneAreaCode = dto.PhoneAreaCode,
            PhoneArea = contact.PhoneArea
        };

        // Act
        _mapper.Map(dto, contact);

        // Assert
        contact.Should().BeEquivalentTo(expectedContact);
    }

    [Fact]
    public void ContactCreatedEventDto_To_Contact_Should_Map_Correctly()
    {
        // Arrange
        var dto = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
            Email = "test@mail.com",
            Name = "Test",
            Phone = "47965232104",
            PhoneAreaCode = 47
        };

        var expectedContact = new Contact
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            PhoneAreaCode = dto.PhoneAreaCode
        };

        // Act
        var result = _mapper.Map<Contact>(dto);

        // Assert
        result.Should().BeEquivalentTo(expectedContact);
    }
}
