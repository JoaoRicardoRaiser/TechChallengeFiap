using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Domain.Entities;
using TechChallenge.GetContact.IntegrationTest.Fixtures;

namespace TechChallenge.GetContact.IntegrationTest.Application.MessageHandlers;

[Collection(nameof(GetContactApiCollectionFixture))]
public class ContactCreatedMessageHandlerTests(WebApplicationFixture webApplicationFixture, DatabaseFixture databaseFixture, RabbitMqFixture rabbitMqFixture)
{
    [Fact]
    public async Task Should_Receive_Created_Contact_Message_And_Persist_On_Database()
    {
        // Arrange

        var @event = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
            Email = "john@mail.com",
            Name = "John Doe",
            Phone = "47987238423",
            PhoneAreaCode = 47
        };

        var messagePublisher = webApplicationFixture.Services.GetRequiredService<IMessagePublisherService<ContactCreatedEventDto>>();
        var messageConsumerService = webApplicationFixture.Services.GetRequiredService<IMessageConsumerService<ContactCreatedEventDto>>();

        // Act
        await messagePublisher.SendMessageAsync(@event);

        await messageConsumerService.ConsumeAsync();
        await Task.Delay(TimeSpan.FromSeconds(5));

        // Assert
        var messagesOnQueue = await rabbitMqFixture.CountMessageFromQueueAsync("gc_contact_created_tests");
        messagesOnQueue.Should().Be(0);

        var expectedSavedContact = new Contact
        {
            Id = @event.Id,
            Name = @event.Name,
            Email = @event.Email,
            Phone = @event.Phone,
            PhoneAreaCode = @event.PhoneAreaCode
        };

        var contactSaved = await databaseFixture.SingleOrDefaultAsync<Contact>(x => x.Id == @event.Id);
        contactSaved.Should().BeEquivalentTo(expectedSavedContact);
    }
}
