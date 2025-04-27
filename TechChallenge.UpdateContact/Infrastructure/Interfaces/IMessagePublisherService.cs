namespace TechChallenge.CreateContact.Infrastructure.Interfaces;

public interface IMessagePublisherService<T>
{
    Task SendMessageAsync(T message);
}
