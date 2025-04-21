using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.Infrastructure.Workers;

public class MessageConsumerWorker<T>(IMessageConsumerService<T> messageConsumerService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await messageConsumerService.StartConsuming();
}