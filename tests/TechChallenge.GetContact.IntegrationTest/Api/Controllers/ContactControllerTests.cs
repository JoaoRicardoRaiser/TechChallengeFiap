using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TechChallenge.CreateContact.Domain.Entities;
using FluentAssertions.Equivalency;
using TechChallenge.CreateContact.IntegrationTest.Fakes;
using TechChallenge.CreateContact.IntegrationTest.Fixtures;

namespace TechChallenge.CreateContact.IntegrationTest.Api.Controllers;

[Collection(nameof(CreateContactApiCollectionFixture))]
public class ContactControllerTests(WebApplicationFixture webAppFixture, DatabaseFixture databaseFixture, RabbitMqFixture rabbitMqFixture)
{
    private readonly HttpClient _httpClient = webAppFixture.CreateClient();

    [Fact]
    public async Task PostAsync_When_Valid_Body_Return_Accepted_Result()
    {
        // Arrange
        var dto = ContactFake.NewPostDto();

        // Act
        var result = await _httpClient.PostAsJsonAsync("contacts", dto);
        var responseContent = await result.Content.ReadAsStringAsync();

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var expectedContactSaved = new Contact
        {
            Name = dto.Name!,
            Email = dto.Email!,
            Phone = dto.PhoneNumber!,
            PhoneAreaCode = int.Parse(dto.PhoneAreaCode),
        };

        var contactSaved = await databaseFixture.SingleOrDefaultAsync<Contact>(x => x.Name == dto.Name && x.Phone == dto.PhoneNumber && x.Email == dto.Email);
        contactSaved.Should().BeEquivalentTo(expectedContactSaved, ContactAssertConfiguration);

        var message = await rabbitMqFixture.GetMessageFromQueueAsync<Contact>("cc_contact_created_test");
        message.Should().BeEquivalentTo(contactSaved);
    }

    private EquivalencyAssertionOptions<Contact> ContactAssertConfiguration(EquivalencyAssertionOptions<Contact> config)
    {
        config.Excluding(c => c.Id);
        config.Excluding(c => c.PhoneArea);

        return config;
    }
}
