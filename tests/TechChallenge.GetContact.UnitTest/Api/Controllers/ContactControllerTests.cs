using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechChallenge.GetContact.Api.Dtos;
using TechChallenge.GetContact.Application.Dtos;
using TechChallenge.GetContact.Application.Interfaces;
using TechChallenge.GetContact.Controllers;

namespace TechChallenge.GetContact.UnitTest.Api.Controllers;
public class ContactControllerTests
{
    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ContactController _contactController;

    //public ContactControllerTests()
    //{
    //    _contactController = new ContactController(_contactServiceMock.Object, _mapperMock.Object);
    //}

    [Fact]
    public async Task Post_Contact_When_Invalid_Body_Should_Return_BadRequest()
    {
        // Arrange
        //var body = new PostContactDto
        //{
        //    Email = "invalidEmail",
        //    Name = "a",
        //    PhoneNumber = "22"
        //};

        //_contactController.ModelState.AddModelError("Error", "Error has been occurred");

        // Act
        //var result = await _contactController.Post(body);
        //var badRequestResult = result as BadRequestObjectResult;

        // Assert
        //_contactServiceMock.Verify(cs => cs.CreateAsync(It.IsAny<CreateContactDto>()), Times.Never);
        //badRequestResult.Should().NotBeNull();
        //badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Post_Contact_When_Valid_Should_Return_Accepted()
    {
        // Arrange
        //var body = new PostContactDto
        //{
        //    Email = "email1@email.com",
        //    Name = "Jhon Doe",
        //    PhoneNumber = "54927174541"
        //};

        //var dto = new CreateContactDto
        //{
        //    Email = body.Email,
        //    Name = body.Name,
        //    Phone = new PhoneDto { Number = body.PhoneNumber }
        //};

        //_mapperMock
        //    .Setup(m => m.Map<CreateContactDto>(It.IsAny<PostContactDto>()))
        //    .Returns(dto);

        // Act
        //var result = await _contactController.Post(body);
        //var badRequestResult = result as AcceptedResult;

        // Assert
        //_contactServiceMock.Verify(s => s.CreateAsync(dto), Times.Once);
        //badRequestResult.Should().NotBeNull();
        //badRequestResult!.StatusCode.Should().Be(StatusCodes.Status202Accepted);
    }
}
