using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechChallenge.DeleteContact.Api.Controllers;
using TechChallenge.DeleteContact.Application.Interfaces;

namespace TechChallenge.DeleteContact.UnitTest.Api.Controllers;

public class ContactControllerTests
{

    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly ContactController _controller;

    public ContactControllerTests()
    {
        _controller = new ContactController(_contactServiceMock.Object);
    }

    [Fact]
    public async Task DeleteAsync_Should_Return_No_Content()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        // Act
        var result = await _controller.DeleteAsync(contactId);
        var resultObject = result as NoContentResult;

        // Assert
        resultObject!.StatusCode.Should().Be(204);

        _contactServiceMock.Verify(s => s.DeleteAsync(contactId), Times.Once);
    }
}
