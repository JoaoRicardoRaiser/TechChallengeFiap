using RabbitMQ.Client;
using TechChallenge.UpdateContact.Application.Dtos.Configuration;

namespace TechChallenge.UpdateContact.Infrastructure.Interfaces;

public interface IRabbitMqService
{
    RabbitMqConfiguration RabbitConfig { get; }

    Task<IConnection> CreateConnectionAsync();
    Task<IChannel> CreateChannelAsync();
    Task DeclareExchangeAsync(string exchangeConfigKey);
    Task DeclareQueueAsync(string queueConfigKey);
    Task ConsumerBindQueueAsync(string consumerConfigKey);
}