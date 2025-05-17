using Microsoft.EntityFrameworkCore;
using Raisersoft.EasyRabbit.Extensions;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Domain.Entities;
using TechChallenge.CreateContact.Domain.Interfaces;
using TechChallenge.CreateContact.Infrastructure.Cache;
using TechChallenge.CreateContact.Infrastructure.Database;
using TechChallenge.CreateContact.Infrastructure.Database.Repositories;
using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.Infrastructure.Extensions;

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
        services.AddScoped<DbContext, CreateContactDbContext>();
        services.AddDbContext<CreateContactDbContext>(
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
        => services.AddPublisher<ContactCreatedEventDto>("ContactCreated");

    private static void AddConsumers(this IServiceCollection services)
    {
        services.AddConsumer<ContactUpdatedEventDto>("ContactUpdated");
        services.AddConsumer<ContactDeletedEventDto>("ContactDeleted");
    }
}
