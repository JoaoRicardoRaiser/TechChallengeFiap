using AutoMapper;
using FluentAssertions;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Profiles;
using TechChallenge.DeleteContact.Domain.Entities;

namespace TechChallenge.DeleteContact.UnitTest.Application.Profiles;

public class ContactProfileTests
{
    private readonly IMapper _mapper;

    public ContactProfileTests()
    {
        _mapper = new MapperConfiguration(x => x.AddProfile<ContactProfile>())
            .CreateMapper();
    }

    [Fact]
    public void Should_Map_ContactCreatedEventDto_To_Contact_Successfully()
    {
        // Arrange
        var dto = new ContactCreatedEventDto
        {
            Email = "test@mail.com",
            Id = Guid.NewGuid(),
            Name = "Test",
            Phone = "47983726345",
            PhoneAreaCode = 47
        };

        // Act
        var result = _mapper.Map<Contact>(dto);

        // Assert
        var expectedContact = new Contact
        {
            Id = dto.Id,
            Deleted = false,
            Email = dto.Email,
            Name = dto.Name,
            Phone = dto.Phone,
            PhoneAreaCode = dto.PhoneAreaCode
        };

        result.Should().BeEquivalentTo(expectedContact);
    }
}
