using AutoMapper;
using FluentAssertions;
using Moq;
using Raisersoft.EasyRabbit.Interfaces;
using System.Linq.Expressions;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Interfaces;
using TechChallenge.DeleteContact.Application.Services;
using TechChallenge.DeleteContact.Domain.Entities;
using TechChallenge.DeleteContact.Domain.Exceptions;
using TechChallenge.DeleteContact.Domain.Interfaces;

namespace TechChallenge.DeleteContact.UnitTest.Application.Services;

public class ContactServiceTests
{
    private readonly Mock<IRepository<Contact>> _contactRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IMessagePublisherService<ContactDeletedEventDto>> _messagePublisherMock = new();

    private readonly IContactService _contactService;

    public ContactServiceTests()
    {
        _contactService = new ContactService(
            _contactRepositoryMock.Object,
            _mapperMock.Object,
            _messagePublisherMock.Object
        ); 
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Contact_Successfully()
    {
        // Arrange
        var dto = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
            Email = "john@mail.com",
            Name = "John",
            Phone = "98732472632",
            PhoneAreaCode = 98
        };

        var contactMapped = new Contact
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            Name = dto.Name,
            Phone = dto.Phone,
            PhoneAreaCode = dto.PhoneAreaCode,
            Deleted = false,
        };

        _mapperMock.Setup(x => x.Map<Contact>(dto)).Returns(contactMapped);

        // Act
        await _contactService.CreateAsync(dto);

        // Assert
        _contactRepositoryMock.Verify(r => r.AddAsync(contactMapped), Times.Once);
        _contactRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Shouldnt_Delete_When_Contact_Not_Exists()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        // Act
        var exception = await Assert.ThrowsAsync< BusinessException>(async() => await _contactService.DeleteAsync(contactId));

        // Assert
        exception.Message.Should().Be($"Contact not exists. Id: {contactId}");
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_From_Database_And_Publish_ContactDeletedEventDto()
    {
        // Arrange
        var contactSaved = new Contact
        {
            Id = Guid.NewGuid(),
            Email = "john@mail.com",
            Name = "John",
            Phone = "98732472632",
            PhoneAreaCode = 98,
            Deleted = false,
        };

        _contactRepositoryMock
            .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contactSaved);

        // Act
        await _contactService.DeleteAsync(contactSaved.Id);

        // Assert
        _contactRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _messagePublisherMock.Verify(p => p.SendMessageAsync(It.Is<ContactDeletedEventDto>(e => e.ContactId == contactSaved.Id)));

        contactSaved.Deleted.Should().BeTrue();
    }
}
