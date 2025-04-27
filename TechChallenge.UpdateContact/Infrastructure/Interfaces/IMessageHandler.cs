namespace TechChallenge.CreateContact.Infrastructure.Interfaces;

public interface IMessageHandler<T>
{
    Task Handle(T message);
}
