using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.Transaction;

namespace Infrastructure.Interfaces;

public interface IOutboxService
{
    Task<bool> SaveOutboxItemAsync(CreateTransactionRequest item, string messageType); 
    Task<bool> SaveOutboxItemAsync(IIntegrationEvent item, string messageType);
}
