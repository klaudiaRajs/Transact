using Transact.Core.Contracts;

namespace Transact.Core.Transactions.Infrastructure;

public interface ITransactionFactory
{
    public Task<Transaction> GetTransaction(int id, CancellationToken cancellationToken);
    bool CreateTransaction(CreateTransactionRequest command);
    Task<IEnumerable<Transaction>> GetTransactions();
}
