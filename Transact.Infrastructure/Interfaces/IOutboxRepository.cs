using Transact.Core.Contracts;

namespace Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task<bool> SaveAsync(CreateTransactionRequest item);
    List<OutboxItem> GetAllUnprocessedMessages();
    Task UpdateProcessedOnAsync(string messageId, DateTime utcNow);
}
