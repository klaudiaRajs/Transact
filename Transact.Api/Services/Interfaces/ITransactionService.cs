using Transact.Core.Contracts;

namespace Transact.Api2.Services;

public interface ITransactionService
{
    Task<Transaction> GetTransactionsById(int id);
    Task<Transaction> CreateTransaction(CreateTransactionRequest createOutboxRequest);
    Task<IEnumerable<Transaction>> GetAllTransactions(); 
}
