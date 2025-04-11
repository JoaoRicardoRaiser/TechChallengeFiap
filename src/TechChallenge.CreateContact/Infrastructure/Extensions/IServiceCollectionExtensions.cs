using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Application.Services;
using TechChallenge.CreateContact.Domain.Entities;
using TechChallenge.CreateContact.Domain.Interfaces;
using TechChallenge.CreateContact.Infrastructure.Cache;
using TechChallenge.CreateContact.Infrastructure.Database;
using TechChallenge.CreateContact.Infrastructure.Database.Repositories;

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
            options => options.UseNpgsql(configuration.GetConnectionString("Postgres")));

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
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
        services.AddPublisher<Contact>("ContactCreated");
        return services;
    }

    private static IServiceCollection AddPublisher<T>(this IServiceCollection services, string configKey)
    {
        services.AddSingleton<IMessagePublisher<T>>(new MessagePublisher<T>(services, configKey));
        return services;
    }
}
