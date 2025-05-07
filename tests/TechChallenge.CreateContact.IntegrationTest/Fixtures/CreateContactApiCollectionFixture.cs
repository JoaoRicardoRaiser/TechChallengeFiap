namespace TechChallenge.CreateContact.IntegrationTest.Fixtures;

[CollectionDefinition(nameof(CreateContactApiCollectionFixture))]
public class CreateContactApiCollectionFixture :
    ICollectionFixture<WebApplicationFixture>,
    ICollectionFixture<DatabaseFixture>,
    ICollectionFixture<RabbitMqFixture>
{
}

