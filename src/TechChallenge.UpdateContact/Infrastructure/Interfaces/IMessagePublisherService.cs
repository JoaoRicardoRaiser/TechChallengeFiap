namespace TechChallenge.UpdateContact.Infrastructure.Interfaces;

public interface IMessagePublisherService<T>
{
    Task SendMessageAsync(T message);
}
