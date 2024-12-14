using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Services;

namespace TechChallenge.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IContactService, ContactService>();
        return services;
    }
}
