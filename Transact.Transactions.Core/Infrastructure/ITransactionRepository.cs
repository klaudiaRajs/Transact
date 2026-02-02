using Transact.Core.Contracts;

namespace Transact.Core.Transactions.Infrastructure;

public interface ITransactionRepository
{
    Task<bool> CreateTransactionAsync(CreateTransactionRequest request); 
    Task<IEnumerable<Transaction>> GetTransactionsAsync();
}
