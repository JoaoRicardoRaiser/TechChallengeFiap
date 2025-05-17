using AutoMapper;
using FluentAssertions;
using Moq;
using Raisersoft.EasyRabbit.Interfaces;
using System.Linq.Expressions;
using TechChallenge.CreateContact.Application.Dtos;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Application.Services;
using TechChallenge.CreateContact.Application.UnitTest.Fixtures;
using TechChallenge.CreateContact.Domain.Entities;
using TechChallenge.CreateContact.Domain.Exceptions;
using TechChallenge.CreateContact.Domain.Interfaces;
using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.UnitTest.Application.Services;

public class ContactServiceTests
{
    private readonly Mock<IRepository<Contact>> _contactRepositoryMock = new();
    private readonly Mock<IPhoneAreaCache> _phoneAreaCacheMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IMessagePublisherService<ContactCreatedEventDto>> _messagePublisherMock = new();
    private readonly IContactService _contactService;

    public ContactServiceTests()
    {
        _contactService = new ContactService(
            _contactRepositoryMock.Object,
            _phoneAreaCacheMock.Object,
            _mapperMock.Object,
            _messagePublisherMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_When_PhoneArea_Not_Exists_Should_Throw_BusinessException()
    {
        // Arrange
        var createContactDto = new CreateContactDto
        {
            Name = "John Doe",
            Email = "johndoe@email.com",
            Phone = new PhoneDto { Number = "47123456789" }
        };

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(
            async () => await _contactService.CreateAsync(createContactDto)
        );

        // Assert
        exception.Message.Should().Be($"Phone area code not exists. Code: {createContactDto.Phone.AreaCode}");

        _contactRepositoryMock.Verify(cc => cc.AddAsync(It.IsAny<Contact>()), Times.Never);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Never);
        _messagePublisherMock.Verify(mp => mp.SendMessageAsync(It.IsAny<ContactCreatedEventDto>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_When_Contact_Alredy_Exists_Should_Throw_BusinessException()
    {
        // Arrange

        _phoneAreaCacheMock
            .Setup(pac => pac.Exists(It.IsAny<int>()))
            .Returns(true);

        var contactSaved = ContactFake.New("John Doe");

        _contactRepositoryMock
            .Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contactSaved);

        var createContactDto = new CreateContactDto
        {
            Name = "John Doe",
            Email = "johndoe@email.com",
            Phone = new PhoneDto { Number = "47123456789" }
        };

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(
            async() => await _contactService.CreateAsync(createContactDto)
        );

        // Assert
        exception.Message.Should().Be($"Contact with this name alredy exists. Name: {createContactDto.Name}");
        
        _contactRepositoryMock.Verify(cc => cc.AddAsync(It.IsAny<Contact>()), Times.Never);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Never);
        _messagePublisherMock.Verify(mp => mp.SendMessageAsync(It.IsAny<ContactCreatedEventDto>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Contact_Correctly()
    {
        // Arrange

        _phoneAreaCacheMock
            .Setup(pac => pac.Exists(It.Is<int>(x => x == 47)))
            .Returns(true);

        var phoneArea = new PhoneArea
        {
            Code = 47,
            Region = "Region"
        };

        _phoneAreaCacheMock
            .Setup(pac => pac.GetByCode(It.Is<int>(x => x == 47)))
            .Returns(phoneArea);

        var createContactDto = new CreateContactDto
        {
            Name = "John Doe",
            Email = "johndoe@email.com",
            Phone = new PhoneDto { Number = "47123456789" }
        };

        var contact = ContactFake.New("John Doe");

        _mapperMock
            .Setup(m => m.Map<Contact>(createContactDto))
            .Returns(contact);


        var @event = new ContactCreatedEventDto
        {
            Email = contact.Email,
            Id = contact.Id,
            Name = contact.Name,
            Phone = contact.Phone,
            PhoneAreaCode = contact.PhoneAreaCode,
        };

        _mapperMock.Setup(m => m.Map<ContactCreatedEventDto>(contact))
            .Returns(@event);

        // Act
        await _contactService.CreateAsync(createContactDto);

        // Assert
        _contactRepositoryMock.Verify(cc => cc.AddAsync(contact), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
        _messagePublisherMock.Verify(mp => mp.SendMessageAsync(@event), Times.Once);
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

    [Fact]
    public async Task UpdateAsync_Shouldnt_Update_When_Contact_Not_Exists()
    {
        // Arrange
        var contactSaved = ContactFake.New("John Doe");

        var dto = new ContactUpdatedEventDto
        {
            ContactId = Guid.NewGuid(),
            Email = "mail@test.com",
            Phone = "1186542415",
            PhoneAreaCode = 11
        };

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _contactService.UpdateAsync(dto));

        // Assert
        exception.Message.Should().Be($"Contact not exists. Id: {dto.ContactId}");

        _mapperMock.Verify(m => m.Map(dto,contactSaved), Times.Never);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Contact_Correctly()
    {
        // Arrange
        var contactSaved = ContactFake.New("John Doe");

        var dto = new ContactUpdatedEventDto
        {
            ContactId = Guid.NewGuid(),
            Email = "mailupdated@test.com",
            Phone = "1186542415",
            PhoneAreaCode = 11
        };

        _contactRepositoryMock
            .Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contactSaved);

        // Act
        await _contactService.UpdateAsync(dto);

        // Assert
        _mapperMock.Verify(m => m.Map(dto, contactSaved), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
    }
}
