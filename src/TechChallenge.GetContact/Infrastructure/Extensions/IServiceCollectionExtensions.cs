using Microsoft.EntityFrameworkCore;
using Raisersoft.EasyRabbit.Extensions;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Domain.Interfaces;
using TechChallenge.GetContact.Infrastructure.Database;
using TechChallenge.GetContact.Infrastructure.Database.Repositories;

namespace TechChallenge.GetContact.Infrastructure.Extensions;

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

    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddEasyRabbitMq();

        services.AddConsumers();

        return services;
    }
  
    private static void AddConsumers(this IServiceCollection services)
    {
        services.AddConsumer<ContactCreatedEventDto>("ContactCreated");
        services.AddConsumer<ContactUpdatedEventDto>("ContactUpdated");
        services.AddConsumer<ContactDeletedEventDto>("ContactDeleted");
    }
}
