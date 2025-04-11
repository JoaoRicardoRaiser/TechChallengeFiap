namespace TechChallenge.CreateContact.Application.Interfaces;

public interface IMessagePublisher<T>
{
    Task SendMessageAsync(T message);
}
