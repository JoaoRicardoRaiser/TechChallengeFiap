using RabbitMQ.Client;
using TechChallenge.CreateContact.Application.Dtos.Configuration;

namespace TechChallenge.CreateContact.Infrastructure.Interfaces;

public interface IRabbitMqService
{
    RabbitMqConfiguration RabbitConfig { get; }

    Task<IConnection> CreateConnectionAsync();
    Task<IChannel> CreateChannelAsync();
    Task DeclareExchangeAsync(string exchangeConfigKey);
    Task DeclareQueueAsync(string queueConfigKey);
    Task ConsumerBindQueueAsync(string consumerConfigKey);
}