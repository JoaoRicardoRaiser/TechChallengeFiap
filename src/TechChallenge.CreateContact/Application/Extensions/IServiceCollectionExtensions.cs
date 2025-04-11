using System.Diagnostics.CodeAnalysis;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Application.Services;

namespace TechChallenge.CreateContact.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMapper();
        services.AddScoped<IContactService, ContactService>();
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
        => services.AddAutoMapper(typeof(Program).Assembly);
}
