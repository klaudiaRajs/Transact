using Transact.Core.Contracts.Infrastructure;

namespace Transact.Core.Contracts.IntegrationEvents;

public class GetOrCreateUserDetailsIntegrationEvent : IIntegrationEvent
{
    User.User UserDetails { get; set; }
    public string CorrelationId { get; set; }
    public string EventType { get; set; }
    public string RoutingKey { get; set; }
    public DateTime OccurredAt { get; init; }
    public string Payload { get; set; }
    public string Exchange { get; set; }
}
