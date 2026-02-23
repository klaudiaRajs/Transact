using Transact.Core.Contracts;

namespace Infrastructure.IntegrationEvents;

public class GetOrCreateUserDetailsIntegrationEvent(string correlationId) : IntegrationEvent(correlationId)
{
    User UserDetails { get; set; }
}
