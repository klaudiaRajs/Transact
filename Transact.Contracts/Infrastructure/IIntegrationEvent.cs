using System.Text.Json.Serialization;

namespace Transact.Core.Contracts.Infrastructure;

public interface IIntegrationEvent
{
    [JsonPropertyName("CorrelationId")]
    public string CorrelationId { get; set; }
    public string EventType { get; set; }
    public string RoutingKey { get; set; }
    public DateTime OccurredAt { get; init; }
    public string Payload { get; set; }
    public string Exchange { get; set; } 
    
}
