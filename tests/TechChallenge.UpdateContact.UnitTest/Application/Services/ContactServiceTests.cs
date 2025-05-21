using AutoMapper;
using FluentAssertions;
using Moq;
using Raisersoft.EasyRabbit.Interfaces;
using System.Linq.Expressions;
using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Application.Services;
using TechChallenge.UpdateContact.Application.UnitTest.Fixtures;
using TechChallenge.UpdateContact.Domain.Entities;
using TechChallenge.UpdateContact.Domain.Exceptions;
using TechChallenge.UpdateContact.Domain.Interfaces;
using TechChallenge.UpdateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.UnitTest.Application.Services;

public class ContactServiceTests
{
    private readonly Mock<IRepository<Contact>> _contactRepositoryMock = new();
    private readonly Mock<IPhoneAreaCache> _phoneAreaCacheMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IMessagePublisherService<ContactUpdatedEventDto>> _messagePublisherMock = new();
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
    public async Task CreateAsync_Should_Create_Contact_When_Input_Is_Valid()
    {
        // Arrange
        var dto = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "johndoe@email.com",
            Phone = "47123456789",
            PhoneAreaCode = 47
        };

        var contact = ContactFake.New("John Doe");

        _mapperMock
            .Setup(x => x.Map<Contact>(dto))
            .Returns(contact);

        // Act
        await _contactService.CreateAsync(dto);

        // Assert
        _mapperMock.Verify(m => m.Map<Contact>(dto), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.AddAsync(It.IsAny<Contact>()), Times.Once);
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
    public async Task DeleteAsync_Should_Delete_Contact_Correctly()
    {
        // Arrange
        var contactSaved = ContactFake.New("John Doe");

        var dto = new ContactDeletedEventDto
        {
            ContactId = contactSaved.Id
        };

        _contactRepositoryMock
            .Setup(cr => cr.SingleOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<string[]?>()))
            .ReturnsAsync(contactSaved);

        // Act
        await _contactService.DeleteAsync(dto);

        // Assert
        _contactRepositoryMock.Verify(cc => cc.Delete(It.IsAny<Contact>()), Times.Once);
        _contactRepositoryMock.Verify(cc => cc.SaveChangesAsync(), Times.Once);
    }
}
