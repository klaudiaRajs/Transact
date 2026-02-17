using Transact.Core.Contracts;

namespace Infrastructure.IntegrationEvents;

public class TriggerTransactionIntegrationEvent(string correlationId) : IntegrationEvent(correlationId)
{
    public CreateTransactionRequest? CreateTransactionRequest { get; set; }
}
