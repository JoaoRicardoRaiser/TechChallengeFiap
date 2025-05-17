using Moq;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Application.MessageHandlers;

namespace TechChallenge.CreateContact.UnitTest.Application.MessageHandlers;

public class ContactUpdatedMessageHandlerTests
{
    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly ContactUpdatedMessageHandler _messageHandler;

    public ContactUpdatedMessageHandlerTests()
    {
        _messageHandler = new ContactUpdatedMessageHandler(_contactServiceMock.Object);
    }

    [Fact]
    public async Task Should_Delete_Contact_Sucessfully()
    {
        // Arrange
        var dto = new ContactUpdatedEventDto
        {
            ContactId = Guid.NewGuid(),
            Email = "mail@test.com",
            Phone = "1186542415",
            PhoneAreaCode = 11
        };

        // Act
        await _messageHandler.HandleAsync(dto);

        // Assert
        _contactServiceMock.Verify(x => x.UpdateAsync(dto), Times.Once);
    }
}
