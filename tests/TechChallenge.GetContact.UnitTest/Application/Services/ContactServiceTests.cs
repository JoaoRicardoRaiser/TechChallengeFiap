using AutoMapper;
using FluentAssertions;
using Moq;
using Raisersoft.EasyRabbit.Interfaces;
using System.Linq.Expressions;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Interfaces;
using TechChallenge.GetContact.Application.Services;
using TechChallenge.GetContact.Application.UnitTest.Fixtures;
using TechChallenge.GetContact.Domain.Entities;
using TechChallenge.GetContact.Domain.Exceptions;
using TechChallenge.GetContact.Domain.Interfaces;

namespace TechChallenge.GetContact.UnitTest.Application.Services;

public class ContactServiceTests
{
    private readonly Mock<IRepository<Contact>> _contactRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IMessagePublisherService<Contact>> _messagePublisherMock = new();
    private readonly IContactService _contactService;

    public ContactServiceTests()
    {
        _contactService = new ContactService(
            _contactRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Save_Contact_When_Valid_Input()
    {
        // Arrange
        var dto = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "johndoe@email.com",
            Phone = "11123456789",
            PhoneAreaCode = 11
        };

        var contact = ContactFake.New("John Doe");

        _mapperMock.Setup(x => x.Map<Contact>(dto))
            .Returns(contact);

        // Act
        await _contactService.CreateAsync(dto);

        // Assert
        _mapperMock.Verify(x => x.Map<Contact>(dto), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.AddAsync(contact), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Throw_BusinessException_When_Contact_Not_Exists()
    {
        // Arrange
        var dto = new ContactUpdatedEventDto
        {
            ContactId = Guid.NewGuid(),
            Name = "John Doe",
            Email = "johndoe@email.com",
            Phone = "11123456789",
            PhoneAreaCode = 11
        };

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(
            async () => await _contactService.UpdateAsync(dto)
        );

        // Assert
        exception.Message.Should().Be($"Contact not exists. Id: {dto.ContactId}");

        _contactRepositoryMock.Verify(cc => cc.AddAsync(It.IsAny<Contact>()), Times.Never);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_When_Contact_Exists_Should_Update_Successfully()
    {
        // Arrange
        var contactSaved = ContactFake.New("John Doe");

        _contactRepositoryMock
            .Setup(x => x.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contactSaved);

        var dto = new ContactUpdatedEventDto
        {
            ContactId = Guid.NewGuid(),
            Name = "John Doe",
            Email = "johndoe@email.com",
            Phone = "11123456789",
            PhoneAreaCode = 11
        };

        // Act
        await _contactService.UpdateAsync(dto);

        // Assert
        _mapperMock.Verify(x => x.Map(dto, contactSaved), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Shouldnt_Delete_Contact_When_Contact_Not_Exists()
    {
        // Arrange
        var contactSaved = ContactFake.New("John Doe");

        var dto = new ContactDeletedEventDto
        {
            ContactId = contactSaved.Id
        };

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(async() => await _contactService.DeleteAsync(dto));

        // Assert
        exception.Message.Should().Be($"Contact not exists. Id: {contactSaved.Id}");

        _contactRepositoryMock.Verify(cc => cc.Delete(contactSaved), Times.Never);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Contact_Correctly()
    {
        // Arrange
        var contactSaved = ContactFake.New("John Doe");

        var dto = new ContactDeletedEventDto
        {
            ContactId = contactSaved.Id
        };

        _contactRepositoryMock
            .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contactSaved);

        // Act
        await _contactService.DeleteAsync(dto);

        // Assert
        _contactRepositoryMock.Verify(cc => cc.Delete(contactSaved), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
    }
}
