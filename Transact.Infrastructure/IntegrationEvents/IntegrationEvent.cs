namespace Infrastructure.IntegrationEvents;

public class IntegrationEvent(Guid EventId)
{
    public Guid EventId { get; init; }
    public string EventType { get; init; } = default!;
    public string RoutingKey { get; init; } = default!;
    public DateTime OccurredAt { get; init; }
    public string Payload { get; init; } = default!;
}
