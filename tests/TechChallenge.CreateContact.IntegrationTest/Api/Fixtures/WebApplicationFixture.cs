using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using TechChallenge.CreateContact.Application.Services;
using TechChallenge.CreateContact.IntegrationTest.Api.Helpers;

namespace TechChallenge.CreateContact.IntegrationTest.Api.Fixtures;

public class WebApplicationFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.IntegrationTests.json")
            .Build();

        builder.UseConfiguration(configuration);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTests");

        RabbitMqHelper
            .CreateExchangesAndQueuesFromConfigurationAsync(configuration, new RabbitMqService(ConfigurationHelper.GetRabbitMqConfigurationOptions()))
            .Wait();
    }
}
