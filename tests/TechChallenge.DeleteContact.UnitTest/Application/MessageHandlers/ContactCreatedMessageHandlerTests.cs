using Moq;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Interfaces;
using TechChallenge.DeleteContact.Application.MessageHandlers;

namespace TechChallenge.DeleteContact.UnitTest.Application.MessageHandlers;

public class ContactCreatedMessageHandlerTests
{
    private readonly Mock<IContactService> _contactServiceMock = new();

    private readonly ContactCreatedMessageHandler _messageHandler;

    public ContactCreatedMessageHandlerTests()
    {
        _messageHandler = new ContactCreatedMessageHandler(_contactServiceMock.Object);
    }

    [Fact]
    public async Task Should_CreateContact_Sucessfully()
    {
        // Arrange
        var dto = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
            Email = "john@mail.com",
            Name = "John",
            Phone = "98732472632",
            PhoneAreaCode = 98
        };

        // Act
        await _messageHandler.HandleAsync(dto);

        // Assert
        _contactServiceMock.Verify(s => s.CreateAsync(dto), Times.Once);
    }

}
