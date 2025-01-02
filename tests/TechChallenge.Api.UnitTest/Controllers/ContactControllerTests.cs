using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechChallenge.Api.Controllers;
using TechChallenge.Api.Dtos;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Api.UnitTest.Controllers;

public class ContactControllerTests
{

    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ContactController _contactController;

    public ContactControllerTests()
    {
        _contactController = new ContactController(_contactServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_Contacts_Should_Return_Ok()
    {
        // Arrange

        var contactsSaved = new List<Contact>
        {
            new() 
            {
                Id = Guid.NewGuid(),
                Email = "email1@email.com",
                Name = "Name1",
                Phone = "99123456789",
                PhoneAreaCode = 99
            },
            new() 
            {
                Id = Guid.NewGuid(),
                Email = "email2@email.com",
                Name = "Name2",
                Phone = "11987654321",
                PhoneAreaCode = 11
            }
        };

        _contactServiceMock
            .Setup(x => x.GetAsync(null))
            .ReturnsAsync(contactsSaved);

        // Act
        var result = await _contactController.Get(null);
        var okResult = result as OkObjectResult;

        // Assert
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult!.Value.Should().Be(contactsSaved);
    }

    [Fact]
    public async Task Post_Contact_When_Invalid_Body_Should_Return_BadRequest()
    {
        // Arrange
        var body = new PostContactDto
        {
            Email = "invalidEmail",
            Name = "a",
            PhoneNumber = "22"
        };

        _contactController.ModelState.AddModelError("Error", "Error has been occurred");

        // Act
        var result = await _contactController.Post(body);
        var badRequestResult = result as BadRequestObjectResult;

        // Assert
        _contactServiceMock.Verify(cs => cs.CreateAsync(It.IsAny<CreateContactDto>()), Times.Never);
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Post_Contact_When_Valid_Should_Return_Accepted()
    {
        // Arrange
        var body = new PostContactDto
        {
            Email = "email1@email.com",
            Name = "Jhon Doe",
            PhoneNumber = "54927174541"
        };

        var dto = new CreateContactDto
        {
            Email = body.Email,
            Name = body.Name,
            Phone = new PhoneDto { Number = body.PhoneNumber }
        };

        _mapperMock
            .Setup(m => m.Map<CreateContactDto>(It.IsAny<PostContactDto>()))
            .Returns(dto);

        // Act
        var result = await _contactController.Post(body);
        var badRequestResult = result as AcceptedResult;

        // Assert
        _contactServiceMock.Verify(s => s.CreateAsync(dto), Times.Once);
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status202Accepted);
    }

    [Fact]
    public async Task Put_Contact_When_Invalid_Body_Should_Return_BadRequest()
    {
        // Arrange
        var body = new PutContactDto
        {
            Email = "invalidEmail",
            PhoneNumber = "22"
        };

        _contactController.ModelState.AddModelError("Error", "Error has been occurred");

        // Act
        var result = await _contactController.Put(Guid.NewGuid(), body);
        var badRequestResult = result as BadRequestObjectResult;

        // Assert
        _contactServiceMock.Verify(cs => cs.UpdateAsync(It.IsAny<UpdateContactDto>()), Times.Never);
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Put_Contact_When_Valid_Body_Should_Return_Accepted()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        var body = new PutContactDto
        {
            Email = "emailupdated@email.com",
            PhoneNumber = "11123456789"
        };

        var dto = new UpdateContactDto
        {
            Email = body.Email,
            Phone = new PhoneDto { Number = body.PhoneNumber }
        };

        _mapperMock
            .Setup(m => m.Map<UpdateContactDto>(It.IsAny<PutContactDto>()))
            .Returns(dto);

        // Act
        var result = await _contactController.Put(contactId, body);
        var badRequestResult = result as AcceptedResult;

        // Assert
        _contactServiceMock.Verify(cs => cs.UpdateAsync(
            It.Is<UpdateContactDto>(
                d => d.ContactId == contactId
                && d.Email == body.Email
                && d.Phone.Number == body.PhoneNumber)), 
            Times.Once);

        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status202Accepted);
    }

    [Fact]
    public async Task Delete_Contact_Should_Return_Accepted()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        // Act
        var result = await _contactController.Delete(contactId);
        var noContentResult = result as NoContentResult;

        // Assert
        _contactServiceMock.Verify(cs => cs.DeleteAsync(contactId), Times.Once);
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
