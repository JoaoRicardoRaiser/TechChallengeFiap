using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Domain.Entities;
using TechChallenge.DeleteContact.IntegrationTest.Fixtures;

namespace TechChallenge.DeleteContact.IntegrationTest.Application.MessageHandlers;

[Collection(nameof(DeleteContactApiCollectionFixture))]
public class ContactUpdatedMessageHandlerTests(WebApplicationFixture webApplicationFixture, DatabaseFixture databaseFixture, RabbitMqFixture rabbitMqFixture)
{

    [Fact]
    public async Task Should_Receive_Update_Contact_Message_And_Update_Contact_Saved()
    {
        // Arrange
        var contactToSave = new Contact
        {
            Deleted = false,
            Email = "test@mail.com",
            Name = "Test",
            Id = Guid.NewGuid(),
            Phone = "47853652147",
            PhoneAreaCode = 47
        };

        await databaseFixture.AddAsync(contactToSave);

        var @event = new ContactUpdatedEventDto
        {
            ContactId = contactToSave.Id,
            Email = "emailupdated@mail.com",
            Phone = "11665985214",
            PhoneAreaCode = 11
        };

        var messagePublisher = webApplicationFixture.Services.GetRequiredService<IMessagePublisherService<ContactUpdatedEventDto>>();
        var messageConsumerService = webApplicationFixture.Services.GetRequiredService<IMessageConsumerService<ContactUpdatedEventDto>>();

        // Act
        await messagePublisher.SendMessageAsync(@event);

        await messageConsumerService.ConsumeAsync();
        await Task.Delay(TimeSpan.FromSeconds(5));

        // Assert
        var messagesOnQueue = await rabbitMqFixture.CountMessageFromQueueAsync("dc_contact_updated_test");
        messagesOnQueue.Should().Be(0);

        var contactSaved = await databaseFixture.SingleOrDefaultAsync<Contact>(x => x.Id == contactToSave.Id);
        var expectedContact = new Contact
        {
            Id = contactToSave.Id,
            Email = @event.Email,
            Name = contactToSave.Name,
            Phone = @event.Phone,
            PhoneAreaCode = @event.PhoneAreaCode
        };

        contactSaved.Should().BeEquivalentTo(expectedContact);
    }
}
