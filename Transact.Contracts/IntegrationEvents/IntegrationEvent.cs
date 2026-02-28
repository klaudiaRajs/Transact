using System.Text.Json.Serialization;
using Transact.Core.Contracts.Infrastructure;

namespace Transact.Core.Contracts.IntegrationEvents;

public class IntegrationEvent : IIntegrationEvent
{
    public IntegrationEvent(string? correlationId = null)
    {
        if (correlationId == null)
        {
            CorrelationId = Guid.NewGuid().ToString();
        }
    }

    public IntegrationEvent()
    {
        
    }
    [JsonPropertyName("CorrelationId")]
    public string CorrelationId { get; set; } = default!;
    public string EventType { get; set; } = default!;
    public string RoutingKey { get; set; }
    public DateTime OccurredAt { get; init; }
    public string Payload { get; set; } = default!;
    public string Exchange { get; set; } 
}
