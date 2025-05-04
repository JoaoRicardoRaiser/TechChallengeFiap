namespace TechChallenge.DeleteContact.IntegrationTest.Fixtures;

[CollectionDefinition(nameof(DeleteContactApiCollectionFixture))]
public class DeleteContactApiCollectionFixture :
    ICollectionFixture<WebApplicationFixture>,
    ICollectionFixture<DatabaseFixture>,
    ICollectionFixture<RabbitMqFixture>
{
}

