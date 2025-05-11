using Raisersoft.EasyRabbit.Interfaces;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Interfaces;
using TechChallenge.GetContact.Application.MessageHandlers;
using TechChallenge.GetContact.Application.Services;

namespace TechChallenge.GetContact.Application.Extensions;

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
        services.AddScoped<IMessageHandler<ContactUpdatedEventDto>, ContactUpdatedMessageHandler>();
        //services.AddScoped<IMessageHandler<Contact>, ContactMessageHandler>();

        return services;
    }    
}
