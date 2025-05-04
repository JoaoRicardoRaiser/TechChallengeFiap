using Raisersoft.EasyRabbit.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Interfaces;
using TechChallenge.DeleteContact.Application.MessageHandlers;
using TechChallenge.DeleteContact.Application.Services;

namespace TechChallenge.DeleteContact.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddApplicationMapper();
        services.AddScoped<IContactService, ContactService>();
        services.AddMessageHandlers();

        return services;
    }

    private static IServiceCollection AddApplicationMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }

    private static IServiceCollection AddMessageHandlers(this IServiceCollection services) 
    {
        services.AddScoped<IMessageHandler<ContactCreatedEventDto>, ContactCreatedMessageHandler>();

        return services;
    }
}
