using Microsoft.EntityFrameworkCore;
using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            dbContext.Database.Migrate();
            logger.LogInformation("Migrations applied with sucess!");
        }
        else
            logger.LogInformation("Database alredy updated. Migrations not applied");
    }

    public static async Task WarmUpCache(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var cacheWarmUpCache = scope.ServiceProvider.GetRequiredService<ICacheWarmUpService>();

        await cacheWarmUpCache.WarmUp();
    }
}
