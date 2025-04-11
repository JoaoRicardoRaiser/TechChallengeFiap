using Microsoft.Extensions.Configuration;
using TechChallenge.CreateContact.Application.Configuration;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Domain.Exceptions;

namespace TechChallenge.CreateContact.IntegrationTest.Api.Helpers;

public static class RabbitMqHelper
{
    public static async Task CreateExchangesAndQueuesFromConfigurationAsync(IConfiguration configuration, IRabbitMqService rabbitMqService)
    {
        var channel = await rabbitMqService.CreateChannelAsync();
        var rabbitMqConfiguration = new RabbitMqConfiguration();
        configuration
            .GetSection("RabbitMq")
            .Bind(rabbitMqConfiguration);

        var defaultExchangeConfigs = rabbitMqConfiguration.Exchanges["Default"];
        var defaultQueueConfigs = rabbitMqConfiguration.Queues["Default"];

        foreach (var publisher in rabbitMqConfiguration.Publishers.Where(x => x.Key != "Default"))
        {
            var exchange = publisher.Value.Exchange ?? throw new ExchangeNotFoundException(publisher.Key);
            var exchangeType = rabbitMqConfiguration.Exchanges[exchange].Type ?? defaultExchangeConfigs.Type;

            await channel.ExchangeDeclareAsync(exchange, exchangeType, false, false);
        }

        foreach(var queue in rabbitMqConfiguration.Queues.Where(x => x.Key != "Default"))
        {
            var exchange = queue.Value.Exchange;
            var routingKey = queue.Value.RoutingKey ?? defaultQueueConfigs.RoutingKey;
            var durable = queue.Value.Durable ?? defaultQueueConfigs.Durable!.Value;
            var exclusive = queue.Value.Exclusive ?? defaultQueueConfigs.Exclusive!.Value;
            var autoDelete = queue.Value.AutoDelete ?? defaultQueueConfigs.AutoDelete!.Value;

            await channel.QueueDeclareAsync(queue.Key,durable, exclusive, autoDelete);
            await channel.QueueBindAsync(queue.Key, exchange, routingKey!);
        }
    }
}
