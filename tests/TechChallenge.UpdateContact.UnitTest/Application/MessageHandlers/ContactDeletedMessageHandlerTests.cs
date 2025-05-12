using Moq;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Application.MessageHandlers;

namespace TechChallenge.CreateContact.UnitTest.Application.MessageHandlers;
public class ContactDeletedMessageHandlerTests
{

    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly ContactDeletedMessageHandler _messageHandler;

    public ContactDeletedMessageHandlerTests()
    {
        _messageHandler = new ContactDeletedMessageHandler(_contactServiceMock.Object);
    }

    [Fact]
    public async Task Should_Delete_Contact_Sucessfully()
    {
        // Arrange
        var dto = new ContactDeletedEventDto
        {
            ContactId = Guid.NewGuid(),
        };

        // Act
        await _messageHandler.HandleAsync(dto);

        // Assert
        _contactServiceMock.Verify(x => x.DeleteAsync(dto), Times.Once);
    }
}
