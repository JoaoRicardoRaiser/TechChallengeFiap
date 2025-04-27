namespace TechChallenge.CreateContact.Infrastructure.Interfaces;

public interface IMessageConsumerService<T>
{
    public Task StartConsuming();
}
