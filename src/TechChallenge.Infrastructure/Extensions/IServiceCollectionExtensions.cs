using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Infrastructure.Cache;
using TechChallenge.Infrastructure.Database;
using TechChallenge.Infrastructure.Database.Repositories;

namespace TechChallenge.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddCache();

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbContext, TechChallengeDbContext>();
        services.AddDbContext<TechChallengeDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    private static void AddCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        
        services.AddScoped<IPhoneAreaCache, PhoneAreaCache>();

        services.AddScoped<ICacheWarmUpService, CacheWarmUpService>();
    }
}
