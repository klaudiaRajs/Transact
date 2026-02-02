using Transact.Core.Contracts;

namespace Infrastructure.Interfaces;

public interface IOutboxService
{
    Task<bool> SaveOutboxItemAsync(CreateTransactionRequest item, string messageType); 
}
