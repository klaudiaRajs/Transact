using Transact.Core.Contracts;

namespace Infrastructure.IntegrationEvents;

public record CreateTransactionIntegrationEvent(string CorrelationId)
{
    public CreateTransactionRequest? CreateTransactionRequest { get; set; } 
    public string CorrelationId { get; init; } = CorrelationId;
}
