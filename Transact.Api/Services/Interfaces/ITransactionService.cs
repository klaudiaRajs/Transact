using Transact.Core.Contracts.Transaction;
using Transact.Core.Contracts.Transactions;

namespace Transact.Api2.Services.Interfaces;

public interface ITransactionService
{
    Task<Transaction> GetTransactionsById(string id);
    Task<Transaction> CreateTransaction(CreateTransactionRequest createOutboxRequest);
    Task<IEnumerable<Transaction>> GetAllTransactions(); 
}
