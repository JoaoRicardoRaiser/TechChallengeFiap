namespace TechChallenge.CreateContact.Application.Configuration;

public class RabbitMqConfiguration
{
    public RabbitMqInfrastructureConfiguration Infrastructure { get; set; } = default!;
    public IDictionary<string, RabbitMqPublishersConfiguration> Publishers { get; set; } = default!;
    public IDictionary<string, RabbitMqExchangesConfiguration> Exchanges { get; set; } = default!;
    public IDictionary<string, RabbitMqQueuesConfiguration> Queues { get; set; } = default!;
}

public class RabbitMqInfrastructureConfiguration
{
    public string HostName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int Port { get; set; }
}

public class RabbitMqPublishersConfiguration
{
    public string? RoutingKey { get; set; } = default!;
    public string Exchange { get; set; } = default!;
}

public class RabbitMqExchangesConfiguration
{
    public string Type { get; set; } = default!;
    public bool? Durable { get; set; } = default!;
    public bool? AutoDelete { get; set; } = default!;
}

public class RabbitMqQueuesConfiguration
{
    public string Exchange { get; set; } = default!;
    public string? RoutingKey { get; set; } = default!;
    public bool? Exclusive { get; set; } = default!;
    public bool? Durable { get; set;} = default!;
    public bool? AutoDelete { get; set; }
}