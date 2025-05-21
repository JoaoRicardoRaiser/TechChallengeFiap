using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechChallenge.UpdateContact.Api.Dtos;
using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Controllers;

namespace TechChallenge.CreateContact.UnitTest.Api.Controllers;
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
    public async Task PutAsync_Contact_When_Invalid_Body_Should_Return_BadRequest()
    {
        // Arrange
        var body = new PutContactDto
        {
            Email = "invalidEmail",
            PhoneNumber = "22"
        };

        _contactController.ModelState.AddModelError("Error", "Error has been occurred");

        // Act
        var result = await _contactController.PutAsync(Guid.NewGuid(), body);
        var badRequestResult = result as BadRequestObjectResult;

        // Assert
        _contactServiceMock.Verify(cs => cs.UpdateAsync(It.IsAny<UpdateContactDto>()), Times.Never);
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task PutAsync_Contact_When_Valid_Body_Should_Return_Accepted()
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
        var result = await _contactController.PutAsync(contactId, body);
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
}
