using Newtonsoft.Json;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Application.Services;
using TechChallenge.CreateContact.IntegrationTest.Api.Helpers;
using Testcontainers.RabbitMq;

namespace TechChallenge.CreateContact.IntegrationTest.Api.Fixtures;

public class RabbitMqFixture
{
    private readonly RabbitMqContainer _dbContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithUsername("guest")
            .WithPassword("guest")
            .WithPortBinding(15672, 15672)
            .WithPortBinding(5672, 5672)
            .Build();
    
    private readonly IRabbitMqService _rabbitMqService;

    public RabbitMqFixture()
    {
        _dbContainer.StartAsync().Wait();
        _rabbitMqService = new RabbitMqService(ConfigurationHelper.GetRabbitMqConfigurationOptions());
    }

    public async Task<T?> GetMessageFromQueueAsync<T>(string queueName)
    {
        var channel = await _rabbitMqService.CreateChannelAsync();

        var result = await channel.BasicGetAsync(queueName, false);

        var body = (result?.Body.ToArray()) ?? throw new Exception($"Has error on get message from queue: {queueName}");
        var text = System.Text.Encoding.UTF8.GetString(body);

        return JsonConvert.DeserializeObject<T>(text);
    }
}
