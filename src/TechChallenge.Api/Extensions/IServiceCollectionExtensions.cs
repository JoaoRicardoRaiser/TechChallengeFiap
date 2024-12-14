using System.Reflection;

namespace TechChallenge.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(
            [
                Assembly.GetExecutingAssembly(), 
                typeof(Application.Extensions.IServiceCollectionExtensions).Assembly
            ]);

        return services;
    }
}
