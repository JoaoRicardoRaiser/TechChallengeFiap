using Moq;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Application.MessageHandlers;

namespace TechChallenge.UpdateContact.UnitTest.Application.MessageHandlers;

public class ContactCreatedMessageHandlerTests
{
    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly ContactCreatedMessageHandler _messageHandler;

    public ContactCreatedMessageHandlerTests()
    {
        _messageHandler = new ContactCreatedMessageHandler(_contactServiceMock.Object);
    }

    [Fact]
    public async Task Should_Create_Contact_Sucessfully()
    {
        // Arrange
        var dto = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
        };

        // Act
        await _messageHandler.HandleAsync(dto);

        // Assert
        _contactServiceMock.Verify(x => x.CreateAsync(dto), Times.Once);
    }
}

