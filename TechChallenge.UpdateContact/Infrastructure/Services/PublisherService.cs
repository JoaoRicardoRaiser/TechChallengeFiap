using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using TechChallenge.CreateContact.Domain.Exceptions;
using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.Infrastructure.Services;

public class PublisherService<T> : IMessagePublisherService<T>
{
    private readonly RabbitMqService _rabbitMqService;
    private readonly string _exchange;
    private readonly string _routingKey;

    public PublisherService(IServiceCollection services, string publisherConfigKey)
    {
        var serviceProvider = services.BuildServiceProvider();
        _rabbitMqService = (RabbitMqService)serviceProvider.GetRequiredService<IRabbitMqService>();

        var exchangeConfiguration = _rabbitMqService.RabbitConfig.Publishers[publisherConfigKey];
        var defaultPublisherConfigs = _rabbitMqService.RabbitConfig.Publishers["Default"];
        _routingKey = exchangeConfiguration.RoutingKey ?? defaultPublisherConfigs.RoutingKey!;

        _exchange = exchangeConfiguration.Exchange
            ?? throw new ExchangeNotFoundException(publisherConfigKey);

        _rabbitMqService.DeclareExchangeAsync(_exchange).Wait();
    }

    public async Task SendMessageAsync(T message)
    {
        var channel = await _rabbitMqService.CreateChannelAsync();
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        await channel.BasicPublishAsync(_exchange, _routingKey, body);
    }
}
