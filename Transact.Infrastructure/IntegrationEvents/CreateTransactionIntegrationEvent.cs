using Transact.Core.Contracts;

namespace Infrastructure.EventBus;

public record CreateTransactionIntegrationEvent(string CorrelationId) : IntegrationEvent(CorrelationId)
{
    public CreateTransactionRequest? CreateTransactionRequest { get; set; } 
    public string CorrelationId { get; init; } = CorrelationId;
}
