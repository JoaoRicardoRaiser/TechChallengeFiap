namespace TechChallenge.DeleteContact.Infrastructure.Interfaces;

public interface IPublisherService<T>
{
    Task SendMessageAsync(T message);
}
