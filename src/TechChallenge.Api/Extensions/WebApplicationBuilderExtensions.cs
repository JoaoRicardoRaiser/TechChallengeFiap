using System.Reflection;

namespace TechChallenge.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile(GetAppsettingsFileName())
            .AddUserSecrets(Assembly.GetExecutingAssembly());

        return builder;
    }

    public static string GetAppsettingsFileName()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? default;
        return environment == default ? "appsettings.json" : string.Format("appsettings.{0}.json", environment);
    }
}
