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
    public async Task Post_Contact_When_Invalid_Body_Should_Return_BadRequest()
    {
        // Arrange
        var body = new PutContactDto
        {
            Email = "invalidEmail",            
            PhoneNumber = "22"
        };

        _contactController.ModelState.AddModelError("Error", "Error has been occurred");

        var id = Guid.NewGuid();

        // Act
        var result = await _contactController.Put(id,body);
        var badRequestResult = result as BadRequestObjectResult;

        // Assert
        _contactServiceMock.Verify(cs => cs.UpdateAsync(It.IsAny<UpdateContactDto>()), Times.Never);
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Put_Contact_When_Valid_Should_Return_Accepted()
    {
        // Arrange
        var body = new PutContactDto
        {
            Email = "email1@email.com",           
            PhoneNumber = "54927174541"
        };

        var dto = new UpdateContactDto
        {
            Email = body.Email,            
            Phone = new PhoneDto { Number = body.PhoneNumber }
        };

        _mapperMock
            .Setup(m => m.Map<UpdateContactDto>(It.IsAny<PutContactDto>()))
            .Returns(dto);

        var id = Guid.NewGuid();

        // Act
        var result = await _contactController.Put(id,body);
        var badRequestResult = result as AcceptedResult;

        // Assert
        _contactServiceMock.Verify(s => s.UpdateAsync(dto), Times.Once);
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status202Accepted);
    }
}
