using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using TechChallenge.CreateContact.Application.Dtos.Configuration;
using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.Infrastructure.Services;

public class RabbitMqService : IRabbitMqService
{
    private readonly string DefaultKey = "Default";

    public readonly RabbitMqConfiguration _rabbitConfig;
    public readonly IConnection _connection;

    public RabbitMqConfiguration RabbitConfig => _rabbitConfig;

    public RabbitMqService(IOptions<RabbitMqConfiguration> rabbitMqOptions)
    {
        _rabbitConfig = rabbitMqOptions.Value;
        _connection = CreateConnectionAsync().GetAwaiter().GetResult();
    }

    public async Task<IConnection> CreateConnectionAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = RabbitConfig.Infrastructure.HostName,
            UserName = RabbitConfig.Infrastructure.UserName,
            Password = RabbitConfig.Infrastructure.Password,
            Port = RabbitConfig.Infrastructure.Port
        };

        var policy = Policy
            .Handle<BrokerUnreachableException>()
            .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        IConnection connection = default!;

        await policy.Execute(async () =>
        {
            connection = await factory.CreateConnectionAsync();
        });

        return connection;
    }

    public async Task<IChannel> CreateChannelAsync()
        => await _connection.CreateChannelAsync();

    public async Task DeclareExchangeAsync(string exchangeSettingsKey)
    {
        var defaultExchangeConfig = RabbitConfig.Exchanges[DefaultKey];
        var exchangeConfig = RabbitConfig.Exchanges[exchangeSettingsKey];

        var exchange = exchangeSettingsKey;
        var type = exchangeConfig.Type ?? defaultExchangeConfig.Type;
        var durable = exchangeConfig.Durable ?? defaultExchangeConfig.Durable!.Value;
        var autoDelete = exchangeConfig.AutoDelete ?? defaultExchangeConfig.AutoDelete!.Value;

        var channel = await CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange, type, durable, autoDelete);
    }

    public async Task DeclareQueueAsync(string queueSettingsKey)
    {
        var defaultQueueConfig = RabbitConfig.Queues[DefaultKey];
        var queueConfig = RabbitConfig.Queues[queueSettingsKey];

        var queue = queueSettingsKey;
        var durable = queueConfig.Durable ?? defaultQueueConfig.Durable!.Value;
        var exclusive = queueConfig.Exclusive ?? defaultQueueConfig.Exclusive!.Value;
        var autoDelete = queueConfig.AutoDelete ?? defaultQueueConfig.AutoDelete!.Value;

        var channel = await CreateChannelAsync();
        await channel.QueueDeclareAsync(queue, durable, exclusive, autoDelete);
    }

    public async Task ConsumerBindQueueAsync(string consumerConfigKey)
    {
        var defaultConsumerConfig = RabbitConfig.Consumers[DefaultKey];
        var consumerConfig = RabbitConfig.Consumers[consumerConfigKey];

        var queue = consumerConfig.Queue;
        var exchange = consumerConfig.Exchange;
        var routingKey = consumerConfig.RoutingKey ?? defaultConsumerConfig.RoutingKey;

        var channel = await CreateChannelAsync();
        await channel.QueueBindAsync(queue, exchange, routingKey);
    }
}