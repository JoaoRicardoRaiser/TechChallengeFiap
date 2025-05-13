namespace TechChallenge.GetContact.IntegrationTest.Fixtures;

[CollectionDefinition(nameof(GetContactApiCollectionFixture))]
public class GetContactApiCollectionFixture :
    ICollectionFixture<WebApplicationFixture>,
    ICollectionFixture<DatabaseFixture>,
    ICollectionFixture<RabbitMqFixture>
{
}

