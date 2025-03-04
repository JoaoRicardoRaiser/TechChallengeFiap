using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Services;
using TechChallenge.Application.UnitTest.Fixtures;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Exceptions;
using TechChallenge.Domain.Interfaces.Repositories;

namespace TechChallenge.Application.UnitTest.Services;

public class ContactServiceTests
{
    private readonly Mock<IRepository<Contact>> _contactRepositoryMock = new();
    private readonly Mock<IPhoneAreaCache> _phoneAreaCacheMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly IContactService _contactService;


    public ContactServiceTests()
    {
        _contactService = new ContactService(
            _contactRepositoryMock.Object,
            _phoneAreaCacheMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task GetAsync_When_Contacts_On_Repository_Should_Return_Contacts_And_Add_On_Cache()
    {
        // Arrange
        var contactsSaved = new List<Contact> { ContactFake.New("John"), ContactFake.New("Marie") };
        _contactRepositoryMock
            .Setup(cr => cr.GetAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contactsSaved);

        // Act
        var contacts = await _contactService.GetAsync(null);

        // Assert
        contacts.Should().BeEquivalentTo(contactsSaved);
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

        // Act
        await _contactService.CreateAsync(createContactDto);

        // Assert
        _contactRepositoryMock.Verify(cc => cc.AddAsync(contact), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_When_PhoneArea_Not_Exists_Should_Throw_BusinessException()
    {
        // Arrange
        var updateContactDto = new UpdateContactDto
        {
            ContactId = Guid.NewGuid(),
            Email = "johndoe@email.com",
            Phone = new PhoneDto { Number = "47123456789" }
        };

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(
            async () => await _contactService.UpdateAsync(updateContactDto)
        );

        // Assert
        exception.Message.Should().Be($"Phone area code not exists. Code: {updateContactDto.Phone.AreaCode}");

        _contactRepositoryMock.Verify(cc => cc.AddAsync(It.IsAny<Contact>()), Times.Never);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_When_Contact_Not_Exists_Should_Throw_BusinessException()
    {
        // Arrange

        _phoneAreaCacheMock
            .Setup(pac => pac.Exists(It.IsAny<int>()))
            .Returns(true);

        var updateContactDto = new UpdateContactDto
        {
            ContactId = Guid.NewGuid(),
            Email = "johndoe@email.com",
            Phone = new PhoneDto { Number = "47123456789" }
        };

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(
            async () => await _contactService.UpdateAsync(updateContactDto)
        );

        // Assert
        exception.Message.Should().Be($"Contact not exists. Id: {updateContactDto.ContactId}");

        _contactRepositoryMock.Verify(cc => cc.AddAsync(It.IsAny<Contact>()), Times.Never);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Contact_Correctly()
    {
        // Arrange
        _phoneAreaCacheMock
            .Setup(pac => pac.Exists(It.IsAny<int>()))
            .Returns(true);

        var phoneArea = new PhoneArea
        {
            Code = 47,
            Region = "Region"
        };

        _phoneAreaCacheMock
            .Setup(pac => pac.GetByCode(It.Is<int>(x => x == 47)))
            .Returns(phoneArea);

        var contact = ContactFake.New("John Doe");
        _contactRepositoryMock
            .Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contact);

        var updateContactDto = new UpdateContactDto
        {
            ContactId = Guid.NewGuid(),
            Email = "emailupdated@email.com",
            Phone = new PhoneDto { Number = "47123456789" }
        };

        contact.Email = "emailupdated@email.com";
        _mapperMock
            .Setup(m => m.Map(updateContactDto, contact))
            .Returns(contact);

        // Act
        await _contactService.UpdateAsync(updateContactDto);

        // Assert
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_When_Contact_Not_Exists_Should_Throw_BusinessException()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        // Act
        var exception = await Assert.ThrowsAsync<BusinessException>(
            async () => await _contactService.DeleteAsync(contactId)
        );

        // Assert
        exception.Message.Should().Be($"Contact not exists. Id: {contactId}");

        _contactRepositoryMock.Verify(cc => cc.Delete(It.IsAny<Contact>()), Times.Never);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Contact_Correctly()
    {
        // Arrange
        var contactSaved = ContactFake.New("John Doe");

        _contactRepositoryMock
            .Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contactSaved);

        // Act
        await _contactService.DeleteAsync(contactSaved.Id);

        // Assert
        _contactRepositoryMock.Verify(cc => cc.Delete(It.IsAny<Contact>()), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
    }
}
