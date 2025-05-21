using Moq;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Interfaces;
using TechChallenge.GetContact.Application.MessageHandlers;

namespace TechChallenge.GetContact.UnitTest.Application.MessageHandlers;

public class ContactUpdatedMessageHandlerTests
{
    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly ContactUpdatedMessageHandler _messageHandler;

    public ContactUpdatedMessageHandlerTests()
    {
        _messageHandler = new ContactUpdatedMessageHandler(_contactServiceMock.Object);
    }

    [Fact]
    public async Task Should_Update_Contact_Sucessfully()
    {
        // Arrange
        var dto = new ContactUpdatedEventDto
        {
            ContactId = Guid.NewGuid(),
            Email = "test@mail.com",
            Name = "John",
            Phone = "11256352417",
            PhoneAreaCode = 11
        };

        // Act
        await _messageHandler.HandleAsync(dto);

        // Assert
        _contactServiceMock.Verify(x => x.UpdateAsync(dto), Times.Once);
    }
}
