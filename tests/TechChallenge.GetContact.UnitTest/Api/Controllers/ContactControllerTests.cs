using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using TechChallenge.GetContact.Application.Interfaces;
using TechChallenge.GetContact.Application.UnitTest.Fixtures;
using TechChallenge.GetContact.Controllers;
using TechChallenge.GetContact.Domain.Entities;

namespace TechChallenge.GetContact.UnitTest.Api.Controllers;
public class ContactControllerTests
{
    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly ContactController _contactController;

    public ContactControllerTests()
    {
        _contactController = new ContactController(_contactServiceMock.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(1)]
    public async Task GetAsync__Should_Return_ContactsSaved(int? phoneAreaNumberFilter)
    {
        // Arrange
        var contactsSaved = new List<Contact>
        {
            ContactFake.New("Joel"),
            ContactFake.New("John")
        };

        _contactServiceMock.Setup(s => s.GetAsync(It.Is<int?>(p => p == phoneAreaNumberFilter)))
            .ReturnsAsync(contactsSaved);

        // Act
        var result = await _contactController.GetAsync(phoneAreaNumberFilter);
        var okObjectResult = result as OkObjectResult;

        // Assert
        _contactServiceMock.Verify(cs => cs.GetAsync(It.Is<int?>(p => p == phoneAreaNumberFilter)), Times.Once);
        okObjectResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okObjectResult!.Value.Should().Be(contactsSaved);
    }
}
