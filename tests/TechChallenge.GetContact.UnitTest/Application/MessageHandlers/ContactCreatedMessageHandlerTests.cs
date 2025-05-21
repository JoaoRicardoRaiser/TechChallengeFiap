using Moq;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Interfaces;
using TechChallenge.GetContact.Application.MessageHandlers;

namespace TechChallenge.GetContact.UnitTest.Application.MessageHandlers;

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
            Email = "test@mail.com",
            Name = "John",
            Phone = "11256352417",
            PhoneAreaCode = 11
        };

        // Act
        await _messageHandler.HandleAsync(dto);

        // Assert
        _contactServiceMock.Verify(x => x.CreateAsync(dto), Times.Once);
    }
}
