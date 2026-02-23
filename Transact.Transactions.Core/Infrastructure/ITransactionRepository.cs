using Infrastructure.IntegrationEvents;
using Transact.Core.Contracts;

namespace Transact.Core.Transactions.Infrastructure;

public interface ITransactionRepository
{
    Task<bool> CreateTransactionAsync(CreateTransactionIntegrationEvent request); 
    Task<IEnumerable<Transaction>> GetTransactionsAsync();
}
