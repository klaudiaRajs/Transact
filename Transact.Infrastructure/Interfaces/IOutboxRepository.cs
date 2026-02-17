using Infrastructure.IntegrationEvents;
using Transact.Core.Contracts;

namespace Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task<bool> SaveTransactionOutboxItem(CreateTransactionRequest item);
    Task<bool> SaveProductOutboxItem(IntegrationEvent item);
    Task<bool> SaveOrchestratorOutboxItem(IntegrationEvent item);
    List<OutboxItem> GetAllUnprocessedMessages();
    Task UpdateProcessedOnAsync(string messageId, DateTime utcNow);
}
