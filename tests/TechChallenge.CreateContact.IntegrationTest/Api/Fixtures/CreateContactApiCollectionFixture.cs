namespace TechChallenge.CreateContact.IntegrationTest.Api.Fixtures;

[CollectionDefinition(nameof(CreateContactApiCollectionFixture))]
public class CreateContactApiCollectionFixture :
    ICollectionFixture<WebApplicationFixture>,
    ICollectionFixture<DatabaseFixture>,
    ICollectionFixture<RabbitMqFixture>
{
}

