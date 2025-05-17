using Moq;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Interfaces;
using TechChallenge.DeleteContact.Application.MessageHandlers;

namespace TechChallenge.DeleteContact.UnitTest.Application.MessageHandlers;

public class ContactUpdatedMessageHandlerTests
{
    private readonly Mock<IContactService> _contactServiceMock = new();

    private readonly ContactUpdatedMessageHandler _messageHandler;

    public ContactUpdatedMessageHandlerTests()
    {
        _messageHandler = new ContactUpdatedMessageHandler(_contactServiceMock.Object);
    }

    [Fact]
    public async Task Should_CreateContact_Sucessfully()
    {
        // Arrange
        var dto = new ContactUpdatedEventDto
        {
            ContactId = Guid.NewGuid(),
            Email = "john@mail.com",
            Phone = "98732472632",
            PhoneAreaCode = 98
        };

        // Act
        await _messageHandler.HandleAsync(dto);

        // Assert
        _contactServiceMock.Verify(s => s.UpdateAsync(dto), Times.Once);
    }

}
