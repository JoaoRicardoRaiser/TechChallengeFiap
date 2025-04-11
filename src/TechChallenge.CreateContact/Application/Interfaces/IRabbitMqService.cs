using RabbitMQ.Client;

namespace TechChallenge.CreateContact.Application.Interfaces;

public interface IRabbitMqService
{
    Task<IChannel> CreateChannelAsync();
}
