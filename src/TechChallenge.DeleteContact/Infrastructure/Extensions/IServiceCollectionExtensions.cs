using Microsoft.EntityFrameworkCore;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Domain.Entities;
using TechChallenge.DeleteContact.Domain.Interfaces;
using TechChallenge.DeleteContact.Infrastructure.Database;
using TechChallenge.DeleteContact.Infrastructure.Database.Repositories;
using TechChallenge.DeleteContact.Infrastructure.Interfaces;
using TechChallenge.DeleteContact.Infrastructure.Services;
using TechChallenge.DeleteContact.Infrastructure.Workers;

namespace TechChallenge.DeleteContact.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddRabbitMq();

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbContext, DeleteContactDbContext>();
        services.AddDbContext<DeleteContactDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqService, RabbitMqService>();

        services.AddPublisher<Contact>("ContactDeleted");

        services.AddConsumer<ContactCreatedEventDto>("ContactCreated");

        return services;
    }

    private static IServiceCollection AddPublisher<T>(this IServiceCollection services, string publisherConfigKey)
    {
        services.AddSingleton<IPublisherService<T>>(new PublisherService<T>(services, publisherConfigKey));
        return services;
    }

    private static IServiceCollection AddConsumer<T>(this IServiceCollection services, string consumerConfigKey)
    {
        var serviceProvider = services.BuildServiceProvider();

        services.AddSingleton<IMessageConsumerService<T>>(new MessageConsumerService<T>(serviceProvider.GetRequiredService<IServiceScopeFactory>(), consumerConfigKey));

        services.AddHostedService<MessageConsumerWorker<T>>();

        return services;
    }
}
