using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TechChallenge.CreateContact.Application.Configuration;

namespace TechChallenge.CreateContact.IntegrationTest.Api.Helpers;

public static class ConfigurationHelper
{

    public static IOptions<RabbitMqConfiguration> GetRabbitMqConfigurationOptions()
    {
        var configuration = GetConfiguration();
        var rabbitMqConfiguration = new RabbitMqConfiguration();
        configuration.Bind("RabbitMq", rabbitMqConfiguration);
        return Options.Create(rabbitMqConfiguration);
    }

    private static IConfigurationRoot GetConfiguration()
        => new ConfigurationBuilder()
        .AddJsonFile("appsettings.IntegrationTests.json")
        .Build();
}
