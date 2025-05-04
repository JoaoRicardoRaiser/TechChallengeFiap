using Microsoft.EntityFrameworkCore;
using Raisersoft.EasyRabbit.Extensions;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Domain.Interfaces;
using TechChallenge.DeleteContact.Infrastructure.Database;
using TechChallenge.DeleteContact.Infrastructure.Database.Repositories;

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
        services.AddEasyRabbitMq();

        services.AddPublishers();
        services.AddConsumers();

        return services;
    }

    private static void AddConsumers(this IServiceCollection services)
        => services.AddConsumer<ContactCreatedEventDto>("ContactCreated");

    private static void AddPublishers(this IServiceCollection services)
        => services.AddPublisher<ContactDeletedEventDto>("ContactDeleted");
}
