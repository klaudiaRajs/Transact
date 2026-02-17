namespace Infrastructure.IntegrationEvents;

public class IntegrationEvent(string correlationId)
{
    public string CorrelationId { get; set; }
    public string EventType { get; set; } = default!;
    public string RoutingKey { get; set; }
    public DateTime OccurredAt { get; init; }
    public string Payload { get; init; } = default!;
    public string Exchange { get; set; } 
}
