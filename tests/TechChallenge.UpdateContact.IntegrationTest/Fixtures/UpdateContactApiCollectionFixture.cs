namespace TechChallenge.UpdateContact.IntegrationTest.Fixtures;

[CollectionDefinition(nameof(UpdateContactApiCollectionFixture))]
public class UpdateContactApiCollectionFixture :
    ICollectionFixture<WebApplicationFixture>,
    ICollectionFixture<DatabaseFixture>,
    ICollectionFixture<RabbitMqFixture>
{
}

