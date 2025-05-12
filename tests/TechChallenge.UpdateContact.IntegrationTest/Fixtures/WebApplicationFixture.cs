using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raisersoft.EasyRabbit.Extensions;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Domain.Entities;

namespace TechChallenge.UpdateContact.IntegrationTest.Fixtures;

public class WebApplicationFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.IntegrationTests.json")
            .Build();

        builder.ConfigureServices(services =>
        {
            services.AddPublisher<Contact>("ContactCreated");
            services.AddPublisher<ContactDeletedEventDto>("ContactDeleted");

            services.AddConsumer<Contact>("ContactCreated");
            services.AddConsumer<ContactDeletedEventDto>("ContactDeleted");

            RemoveIHostServices(services);
        });
        builder.UseConfiguration(configuration);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTests");
    }

    private static void RemoveIHostServices(IServiceCollection services)
    {
        var descriptorsToRemove = services
            .Where(d => typeof(IHostedService).IsAssignableFrom(d.ServiceType))
            .ToList();

        foreach (var descriptor in descriptorsToRemove)
            services.Remove(descriptor);
    }
}
