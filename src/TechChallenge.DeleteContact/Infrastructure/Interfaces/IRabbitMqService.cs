using RabbitMQ.Client;
using TechChallenge.DeleteContact.Application.Dtos.Configuration;

namespace TechChallenge.DeleteContact.Infrastructure.Interfaces;

public interface IRabbitMqService
{
    RabbitMqConfiguration RabbitConfig { get; }

    Task<IConnection> CreateConnectionAsync();
    Task<IChannel> CreateChannelAsync();
    Task DeclareExchangeAsync(string exchangeConfigKey);
    Task DeclareQueueAsync(string queueConfigKey);
    Task ConsumerBindQueueAsync(string consumerConfigKey);
}
