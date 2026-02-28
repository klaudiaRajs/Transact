namespace Transact.Core.Contracts.Infrastructure;

public class RabbitMqOptions
{
    public string Uri { get; init; } = default!;
    public string ClientName { get; init; } = default!;
}
