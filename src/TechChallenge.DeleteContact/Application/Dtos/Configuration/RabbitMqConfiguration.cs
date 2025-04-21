namespace TechChallenge.DeleteContact.Application.Dtos.Configuration;

public class RabbitMqConfiguration
{
    public RabbitMqInfrastructureConfiguration Infrastructure { get; set; } = default!;
    public IDictionary<string, RabbitMqPublisherConfiguration> Publishers { get; set; } = default!;
    public IDictionary<string, RabbitMqConsumerConfiguration> Consumers { get; set; } = default!;
    public IDictionary<string, RabbitMqExchangesConfiguration> Exchanges { get; set; } = default!;
    public IDictionary<string, RabbitMqQueueConfiguration> Queues { get; set; } = default!;
}

public class RabbitMqInfrastructureConfiguration
{
    public string HostName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int Port { get; set; }
}

public class RabbitMqPublisherConfiguration
{
    public string? RoutingKey { get; set; } = default!;
    public string Exchange { get; set; } = default!;
}

public class RabbitMqConsumerConfiguration
{
    public string Exchange { get; set; } = default!;
    public string Queue { get; set; } = default!;
    public string RoutingKey { get; set; } = default!;
}

public class RabbitMqExchangesConfiguration
{
    public string Type { get; set; } = default!;
    public bool? Durable { get; set; } = default!;
    public bool? AutoDelete { get; set; } = default!;
}

public class RabbitMqQueueConfiguration
{
    public string Exchange { get; set; } = default!;
    public string? RoutingKey { get; set; } = default!;
    public bool? Exclusive { get; set; } = default!;
    public bool? Durable { get; set; } = default!;
    public bool? AutoDelete { get; set; }
}
