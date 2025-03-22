using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using TechChallenge.Api.IntegrationTest.Fakes;
using TechChallenge.Api.IntegrationTest.Fixtures;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Api.IntegrationTest.Controllers;

[Collection(nameof(ApiCollectionFixture))]
public class ContactControllerTests(WebApplicationFixture webAppFixture, DatabaseFixture databaseFixture)
{
    private readonly HttpClient _httpClient = webAppFixture.CreateClient();

    [Fact]
    public async Task GetAsync_When_Filter_By_PhoneAreaCode_Filters_Should_Return_Filtered_Contacts()
    {
        // Arrange
        var phoneAreaCode = 11;

        // Act
        var result = await _httpClient.GetAsync($"contacts?phoneAreaNumber={phoneAreaCode}");
        var content = await result.Content.ReadAsStringAsync();

        //Assert
        var expectedContent = await GetExpectedJson(nameof(GetAsync_When_Filter_By_PhoneAreaCode_Filters_Should_Return_Filtered_Contacts));

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be(expectedContent);
    }

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
        
        var contactSaved = await databaseFixture.SingleOrDefaultAsync<Contact>(x => x.Name == dto.Name && x.Phone == dto.PhoneNumber && x.Email == dto.Email);
        contactSaved.Should().NotBeNull();
    }

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

    [Fact]
    public async Task DeleteAsync_When_Valid_Body_Return_Accepted_Result()
    {
        // Arrange
        var contactSaved = ContactFake.New("Mike");
        await databaseFixture.AddAsync(contactSaved);

        // Act
        var result = await _httpClient.DeleteAsync($"contacts/{contactSaved.Id}");
        var responseContent = await result.Content.ReadAsStringAsync();

        //Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var contactSavedUpdated = await databaseFixture.SingleOrDefaultAsync<Contact>(x => x.Id == contactSaved.Id);
        contactSavedUpdated.Should().BeNull();
    }


    public static async Task<string> GetExpectedJson(string jsonName)
    {
        var filePath = @$"Controllers\Contact\ExpectedResponses\{jsonName}.json";
        return await File.ReadAllTextAsync(filePath);
    }
}
