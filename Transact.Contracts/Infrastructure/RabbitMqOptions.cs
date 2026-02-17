namespace Transact.Core.Contracts;

public class RabbitMqOptions
{
    public string Uri { get; init; } = default!;
    public string ClientName { get; init; } = default!;
}
