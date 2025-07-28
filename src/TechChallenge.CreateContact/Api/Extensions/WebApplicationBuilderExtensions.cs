using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TechChallenge.CreateContact.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(GetAppsettingsFileName())
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .AddEnvironmentVariables();

        return builder;
    }

    public static string GetAppsettingsFileName()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? default;
        return environment == default ? "appsettings.json" : string.Format("appsettings.{0}.json", environment);
    }
}
