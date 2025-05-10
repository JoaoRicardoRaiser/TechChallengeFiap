using TechChallenge.UpdateContact.Domain.Entities;

namespace TechChallenge.UpdateContact.Infrastructure.Interfaces;

public interface IMessageHandler<T>
{
    Task Handle(T message);    
}
