namespace TechChallenge.DeleteContact.Infrastructure.Interfaces;

public interface IMessageHandler<T>
{
    Task Handle(T message);
}
