using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TechChallenge.UpdateContact.Domain.Entities;
using FluentAssertions.Equivalency;
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
    }

    private EquivalencyAssertionOptions<Contact> ContactAssertConfiguration(EquivalencyAssertionOptions<Contact> config)
    {
        config.Excluding(c => c.Id);
        config.Excluding(c => c.PhoneArea);

        return config;
    }
}
