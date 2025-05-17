using FluentAssertions;
using FluentAssertions.Equivalency;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Domain.Entities;
using TechChallenge.DeleteContact.IntegrationTest.Fakes;
using TechChallenge.DeleteContact.IntegrationTest.Fixtures;

namespace TechChallenge.DeleteContact.IntegrationTest.Api.Controllers;

[Collection(nameof(DeleteContactApiCollectionFixture))]
public class ContactControllerTests(WebApplicationFixture webAppFixture, DatabaseFixture databaseFixture, RabbitMqFixture rabbitMqFixture)
{

    [Fact]
    public async Task DeleteAsync_Should_Remove_Contact_From_Database_And_Publish_Deleted_Contact_Message()
    {
        // Arrange
        var httpClient = webAppFixture.CreateClient();
        var contact = ContactFake.New("Jonas");
        await databaseFixture.AddAsync(contact);

        // Act
        var result = await httpClient.DeleteAsync($"contacts/{contact.Id}");

        // Assert

        result.EnsureSuccessStatusCode();
        
        var contactSaved = await databaseFixture.SingleOrDefaultAsync<Contact>(c => c.Id == contact.Id);

        contactSaved!.Deleted.Should().BeTrue();

        var messageOnQueue = await rabbitMqFixture.GetMessageFromQueueAsync<ContactDeletedEventDto>("dc_contact_deleted_test");

        messageOnQueue!.ContactId.Should().Be(contact.Id);

    }
}
