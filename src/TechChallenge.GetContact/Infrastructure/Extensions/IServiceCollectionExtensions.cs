using Microsoft.EntityFrameworkCore;
using Raisersoft.EasyRabbit.Extensions;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Domain.Entities;
using TechChallenge.GetContact.Domain.Interfaces;
using TechChallenge.GetContact.Infrastructure.Cache;
using TechChallenge.GetContact.Infrastructure.Database;
using TechChallenge.GetContact.Infrastructure.Database.Repositories;
using TechChallenge.GetContact.Infrastructure.Interfaces;

namespace TechChallenge.GetContact.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddCache();
        services.AddRabbitMq();

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbContext, GetContactDbContext>();
        services.AddDbContext<GetContactDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("Postgres"), 
            npgsqlOptionsAction => npgsqlOptionsAction.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null)));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheWarmUpService, CacheWarmUpService>();
        services.AddScoped<IPhoneAreaCache, PhoneAreaCache>();

        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddEasyRabbitMq();

        services.AddPublishers();

        services.AddConsumers();

        return services;
    }
   
    private static void AddPublishers(this IServiceCollection services)
        => services.AddPublisher<Contact>("ContactRetrieved");

    private static void AddConsumers(this IServiceCollection services)
    {
        services.AddConsumer<ContactDeletedEventDto>("ContactDeleted");
        services.AddConsumer<ContactCreatedEventDto>("ContactCreated");
        services.AddConsumer<ContactUpdatedEventDto>("ContactUpdated");
    }

    //private static IServiceCollection AddConsumer<T>(this IServiceCollection services, string consumerConfigKey)
    //{
    //    var serviceProvider = services.BuildServiceProvider();

    //    services.AddSingleton<IMessageConsumerService<T>>(new MessageConsumerService<T>(serviceProvider.GetRequiredService<IServiceScopeFactory>(), consumerConfigKey));
    //    services.AddHostedService<MessageConsumerWorker<T>>();

    //    return services;
    //}

    //private static IServiceCollection AddPublisher<T>(this IServiceCollection services, string publisherConfigKey)
    //{
    //    services.AddSingleton<IMessagePublisherService<Contact>>(new PublisherService<Contact>(services, publisherConfigKey));

    //    return services;
    //}

}
