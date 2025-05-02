using System.Diagnostics.CodeAnalysis;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Application.MessageHandlers;
using TechChallenge.UpdateContact.Application.Services;
using TechChallenge.UpdateContact.Infrastructure.Interfaces;

namespace TechChallenge.UpdateContact.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMapper();
        services.AddScoped<IContactService, ContactService>();
        services.AddMessageHandlers();
        return services;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
        => services.AddAutoMapper(typeof(Program).Assembly);

    private static IServiceCollection AddMessageHandlers(this IServiceCollection services)
    {
        services.AddScoped<IMessageHandler<ContactDeletedEventDto>, ContactDeletedMessageHandler>();
        services.AddScoped<IMessageHandler<ContactCreatedEventDto>, ContactCreatedMessageHandler>();

        return services;
    }
}
