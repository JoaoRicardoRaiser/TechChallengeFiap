using TechChallenge.DeleteContact.Infrastructure.Interfaces;

namespace TechChallenge.DeleteContact.Infrastructure.Workers;

public class MessageConsumerWorker<T>(IMessageConsumerService<T> messageConsumerService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await messageConsumerService.StartConsuming();
}
