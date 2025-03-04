namespace TechChallenge.Api.IntegrationTest.Fixtures;

[CollectionDefinition(nameof(ApiCollectionFixture))]
public class ApiCollectionFixture:
    ICollectionFixture<WebApplicationFixture>,
    ICollectionFixture<DatabaseFixture>
{
}
