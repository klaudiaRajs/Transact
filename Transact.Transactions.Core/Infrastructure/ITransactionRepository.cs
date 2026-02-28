using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.Transactions;

namespace Transact.Core.Transactions.Infrastructure;

public interface ITransactionRepository
{
    Task<bool> CreateTransactionAsync(IIntegrationEvent transaction); 
    Task<IEnumerable<Transaction>> GetTransactionsAsync();
    Task <Transaction> GetTransactionByIdAsync(string id);
}
