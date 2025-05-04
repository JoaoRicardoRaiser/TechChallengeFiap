using Raisersoft.EasyRabbit.Interfaces;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Application.MessageHandlers;
using TechChallenge.CreateContact.Application.Services;

namespace TechChallenge.CreateContact.Application.Extensions;

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

        return services;
    }
}
