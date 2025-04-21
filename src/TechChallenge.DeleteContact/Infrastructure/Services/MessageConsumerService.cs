using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using TechChallenge.DeleteContact.Infrastructure.Interfaces;

namespace TechChallenge.DeleteContact.Infrastructure.Services;

public class MessageConsumerService<T>(IServiceScopeFactory serviceScopeFactory, string consumerConfigKey) : IMessageConsumerService<T>
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public async Task StartConsuming()
    {
        var scope = _serviceScopeFactory.CreateScope();

        var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
        var consumerConfig = rabbitMqService.RabbitConfig.Consumers[consumerConfigKey];

        await rabbitMqService.DeclareExchangeAsync(consumerConfig.Exchange);
        await rabbitMqService.DeclareQueueAsync(consumerConfig.Queue);
        await rabbitMqService.ConsumerBindQueueAsync(consumerConfigKey);

        var channel = await rabbitMqService.CreateChannelAsync();

        var messageHandler = scope.ServiceProvider.GetRequiredService<IMessageHandler<T>>();

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                var dto = JsonConvert.DeserializeObject<T>(messageJson);

                await messageHandler.Handle(dto!);

                await channel.BasicAckAsync(ea.DeliveryTag, false);

            }
            catch
            {
                await channel.BasicRejectAsync(ea.DeliveryTag, false);
            }
        };

        await channel.BasicConsumeAsync(queue: consumerConfig.Queue, autoAck: false, consumer: consumer);
    }
}
