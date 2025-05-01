using TechChallenge.UpdateContact.Infrastructure.Interfaces;

namespace TechChallenge.UpdateContact.Infrastructure.Workers;

public class MessageConsumerWorker<T>(IMessageConsumerService<T> messageConsumerService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await messageConsumerService.StartConsuming();
}