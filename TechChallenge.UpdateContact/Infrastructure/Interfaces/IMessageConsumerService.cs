namespace TechChallenge.UpdateContact.Infrastructure.Interfaces;

public interface IMessageConsumerService<T>
{
    public Task StartConsuming();
}
