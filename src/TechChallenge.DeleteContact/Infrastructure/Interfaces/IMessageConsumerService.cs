namespace TechChallenge.DeleteContact.Infrastructure.Interfaces;

public interface IMessageConsumerService<T>
{
    public Task StartConsuming();
}
