using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Domain.Entities;
using TechChallenge.UpdateContact.IntegrationTest.Fakes;
using TechChallenge.UpdateContact.IntegrationTest.Fixtures;

namespace TechChallenge.UpdateContact.IntegrationTest.Api.Controllers;

[Collection(nameof(UpdateContactApiCollectionFixture))]
public class ContactControllerTests(WebApplicationFixture webAppFixture, DatabaseFixture databaseFixture, RabbitMqFixture rabbitMqFixture)
{
    private readonly HttpClient _httpClient = webAppFixture.CreateClient();

    [Fact]
    public async Task PutAsync_When_Valid_Body_Return_Accepted_Result()
    {
        // Arrange
        var contactSaved = ContactFake.New("Drew");
        await databaseFixture.AddAsync(contactSaved);

        var dto = ContactFake.NewPutDto();

        // Act
        var result = await _httpClient.PutAsJsonAsync($"contacts/{contactSaved.Id}", dto);
        var responseContent = await result.Content.ReadAsStringAsync();

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var contactSavedUpdated = await databaseFixture.SingleOrDefaultAsync<Contact>(x => x.Id == contactSaved.Id);
        contactSavedUpdated.Should().NotBeNull();
        contactSavedUpdated!.Email.Should().Be(dto.Email);
        contactSavedUpdated!.Phone.Should().Be(dto.PhoneNumber);

        var message = await rabbitMqFixture.GetMessageFromQueueAsync<ContactUpdatedEventDto>("cu_contact_updated_tests");
        
        var expectedMessage = new ContactUpdatedEventDto
        {
            ContactId = contactSavedUpdated.Id,
            Email = contactSavedUpdated.Email,
            Name = contactSavedUpdated.Name,
            Phone = contactSavedUpdated.Phone,
            PhoneAreaCode = contactSavedUpdated.PhoneAreaCode
        };

        message.Should().BeEquivalentTo(expectedMessage);
    }
}
