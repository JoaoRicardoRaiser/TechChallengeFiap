using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Domain.Entities;
using TechChallenge.CreateContact.IntegrationTest.Fakes;
using TechChallenge.CreateContact.IntegrationTest.Fixtures;

namespace TechChallenge.CreateContact.IntegrationTest.Application.MessageHandlers;


[Collection(nameof(CreateContactApiCollectionFixture))]
public class ContactDeletedMessageHandlerTests(WebApplicationFixture webApplicationFixture, DatabaseFixture databaseFixture, RabbitMqFixture rabbitMqFixture)
{

    [Fact]
    public async Task Should_Receive_Delete_Contact_Message_And_Remove_From_Database()
    {
        // Arrange
        var contactToSave = ContactFake.New("John");

        await databaseFixture.AddAsync(contactToSave);

        var @event = new ContactDeletedEventDto
        {
            ContactId = contactToSave.Id
        };

        var messagePublisher = webApplicationFixture.Services.GetRequiredService<IMessagePublisherService<ContactDeletedEventDto>>();
        var messageConsumerService = webApplicationFixture.Services.GetRequiredService<IMessageConsumerService<ContactDeletedEventDto>>();

        // Act
        await messagePublisher.SendMessageAsync(@event);
        
        await messageConsumerService.ConsumeAsync();
        await Task.Delay(TimeSpan.FromSeconds(5));

        // Assert
        var messagesOnQueue = await rabbitMqFixture.CountMessageFromQueueAsync("cc_contact_deleted_test");
        messagesOnQueue.Should().Be(0);

        var contactSaved = await databaseFixture.SingleOrDefaultAsync<Contact>(x => x.Id == contactToSave.Id);
        contactSaved.Should().BeNull();
    }
}
