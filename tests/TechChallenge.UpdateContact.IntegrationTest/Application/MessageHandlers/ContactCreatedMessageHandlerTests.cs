using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Domain.Entities;
using TechChallenge.UpdateContact.IntegrationTest.Fakes;
using TechChallenge.UpdateContact.IntegrationTest.Fixtures;

namespace TechChallenge.UpdateContact.IntegrationTest.Application.MessageHandlers;

[Collection(nameof(UpdateContactApiCollectionFixture))]
public class ContactCreatedMessageHandlerTests(WebApplicationFixture webApplicationFixture, DatabaseFixture databaseFixture, RabbitMqFixture rabbitMqFixture)
{
    [Fact]
    public async Task Should_Receive_Create_Contact_Message_And_Insert_Into_Database()
    {
        // Arrange
        var @event = new ContactCreatedEventDto
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Email = "john@mail.com",
            PhoneAreaCode = 11,
            Phone = "998341844"

        };

        var publisher = webApplicationFixture.Services.GetRequiredService<IMessagePublisherService<ContactCreatedEventDto>>();
        var consumer = webApplicationFixture.Services.GetRequiredService<IMessageConsumerService<ContactCreatedEventDto>>();

        // Act
        await publisher.SendMessageAsync(@event);
        await consumer.ConsumeAsync();

        // Assert
        var saved = await databaseFixture
            .SingleOrDefaultAsync<Contact>(c => c.Id == @event.Id);
        saved.Should().NotBeNull();
        saved!.Email.Should().Be(@event.Email);
        saved.Phone.Should().Be(@event.Phone);
    }
}

