using Microsoft.Extensions.Configuration;

namespace TechChallenge.DeleteContact.IntegrationTest.Helpers;

public static class ConfigurationHelper
{
    public static IConfigurationRoot GetConfiguration()
        => new ConfigurationBuilder()
        .AddJsonFile("appsettings.IntegrationTests.json")
        .Build();
}
