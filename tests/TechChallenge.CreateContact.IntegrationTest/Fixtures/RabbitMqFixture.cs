using Newtonsoft.Json;
using Raisersoft.EasyRabbit.Services;
using TechChallenge.CreateContact.IntegrationTest.Helpers;
using Testcontainers.RabbitMq;

namespace TechChallenge.CreateContact.IntegrationTest.Fixtures;

public class RabbitMqFixture
{
    private readonly RabbitMqContainer _dbContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithUsername("guest")
            .WithPassword("guest")
            .WithPortBinding(12324, 15672)
            .WithPortBinding(2324, 5672)
            .Build();

    public readonly RabbitMqService RabbitMqService;

    public RabbitMqFixture()
    {
        _dbContainer.StartAsync().Wait();
        RabbitMqService = new RabbitMqService(ConfigurationHelper.GetConfiguration());
    }

    public async Task<T?> GetMessageFromQueueAsync<T>(string queueName)
    {
        var channel = await RabbitMqService.CreateChannelAsync();

        var result = await channel.BasicGetAsync(queueName, false);

        var body = (result?.Body.ToArray()) ?? throw new Exception($"Has error on get message from queue: {queueName}");
        var text = System.Text.Encoding.UTF8.GetString(body);

        return JsonConvert.DeserializeObject<T>(text);
    }

    public async Task<int> CountMessageFromQueueAsync(string queueName)
    {
        var channel = await RabbitMqService.CreateChannelAsync();

        var count = await channel.MessageCountAsync(queueName);

        return (int)count;
    }
}
