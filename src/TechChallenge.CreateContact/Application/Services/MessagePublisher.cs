using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using TechChallenge.CreateContact.Application.Configuration;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Domain.Exceptions;

namespace TechChallenge.CreateContact.Application.Services;

public class MessagePublisher<T> : IMessagePublisher<T>, IDisposable
{
    private readonly IChannel _channel;
    private readonly string _exchange;
    private readonly string _routingKey;

    public MessagePublisher(IServiceCollection services, string keySetting)
    {
        var serviceProvider = services.BuildServiceProvider();
        _channel = serviceProvider.GetRequiredService<IRabbitMqService>().CreateChannelAsync().GetAwaiter().GetResult();
        
        var rabbitMqConfiguration = GetRabbitMqConfiguration(serviceProvider);
        var exchangeConfiguration = rabbitMqConfiguration.Publishers[keySetting];
        
        _exchange = exchangeConfiguration.Exchange 
            ?? throw new ExchangeNotFoundException(keySetting);

        var defaultPublisherConfigs = rabbitMqConfiguration.Publishers["Default"];
        _routingKey = exchangeConfiguration.RoutingKey ?? defaultPublisherConfigs.RoutingKey!;

        CreateExchangeIfNotExists(rabbitMqConfiguration).Wait();
    }

    public async Task SendMessageAsync(T message)
    {           
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        
        await _channel.BasicPublishAsync(_exchange, _routingKey, body);
    }

    public void Dispose()
    {
        _channel.Dispose();
        GC.SuppressFinalize(this);
    }

    private static RabbitMqConfiguration GetRabbitMqConfiguration(IServiceProvider serviceProvider)
    {
        var rabbitMqConfiguration = new RabbitMqConfiguration();
        serviceProvider.GetRequiredService<IConfiguration>()
            .GetSection("RabbitMq")
            .Bind(rabbitMqConfiguration);

        return rabbitMqConfiguration;
    }

    private async Task CreateExchangeIfNotExists(RabbitMqConfiguration rabbitMqConfiguration)
    {
        var exchangeConfig = rabbitMqConfiguration.Exchanges[_exchange];
        var defaultExchangeConfig = rabbitMqConfiguration.Exchanges["Default"];

        var exchangeType = exchangeConfig?.Type ?? defaultExchangeConfig.Type;
        var durable = exchangeConfig?.Durable ?? defaultExchangeConfig.Durable!.Value;

        try
        {
            await _channel.ExchangeDeclarePassiveAsync(_exchange);
        }
        catch (OperationInterruptedException)
        {
            // Se a exchange não existir, cria
            await _channel.ExchangeDeclareAsync(_exchange, exchangeType, durable);
            Console.WriteLine($"Exchange '{_exchange}' created with success.");
        }
    }
}
