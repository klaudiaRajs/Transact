using Transact.Core.Contracts.Infrastructure;

namespace Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task<bool> SaveItemToOutbox(IIntegrationEvent item); 
    Task<List<OutboxItem>> GetAllUnprocessedMessages();
    Task<bool> UpdateProcessedOnAsync(string messageId, IIntegrationEvent item);
}
