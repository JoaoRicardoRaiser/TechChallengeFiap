using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TechChallenge.CreateContact.Application.Configuration;
using TechChallenge.CreateContact.Application.Interfaces;

namespace TechChallenge.CreateContact.Application.Services;

public class RabbitMqService(IOptions<RabbitMqConfiguration> rabbitMqOptions) : IRabbitMqService
{
    private RabbitMqConfiguration _rabbitMqConfiguration = rabbitMqOptions.Value;

    public async Task<IChannel> CreateChannelAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqConfiguration.Infrastructure.HostName,
            UserName = _rabbitMqConfiguration.Infrastructure.UserName,
            Password = _rabbitMqConfiguration.Infrastructure.Password,
            Port = _rabbitMqConfiguration.Infrastructure.Port
        };

        var connection = await factory.CreateConnectionAsync();
        return await connection.CreateChannelAsync();
    }
}
