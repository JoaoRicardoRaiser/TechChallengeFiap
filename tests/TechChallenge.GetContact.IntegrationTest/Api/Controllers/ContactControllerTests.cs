using FluentAssertions;
using System.Net;
using TechChallenge.GetContact.IntegrationTest.Fixtures;

namespace TechChallenge.GetContact.IntegrationTest.Api.Controllers;

[Collection(nameof(GetContactApiCollectionFixture))]
public class ContactControllerTests(WebApplicationFixture webAppFixture)
{
    private readonly HttpClient _httpClient = webAppFixture.CreateClient();

    [Fact]
    public async Task GetAsync_When_Filter_By_PhoneAreaCode_Filters_Should_Return_Filtered_Contacts()
    {
        // Arrange
        var phoneAreaCode = 99;

        // Act
        var result = await _httpClient.GetAsync($"contacts?phoneAreaNumber={phoneAreaCode}");
        var content = await result.Content.ReadAsStringAsync();

        //Assert
        var expectedContent = await GetExpectedJson(nameof(GetAsync_When_Filter_By_PhoneAreaCode_Filters_Should_Return_Filtered_Contacts));

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be(expectedContent);
    }

    public static async Task<string> GetExpectedJson(string jsonName)
    {
        var filePath = @$"Api\Controllers\ExpectedResponses\{jsonName}.json";
        return await File.ReadAllTextAsync(filePath);
    }

}
