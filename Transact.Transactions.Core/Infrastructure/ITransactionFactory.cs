using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.Transactions;

namespace Transact.Core.Transactions.Infrastructure;

public interface ITransactionFactory
{
    public Task<Transaction> GetTransaction(string id);
    bool CreateTransaction(IIntegrationEvent createTransactionEvent);
    Task<IEnumerable<Transaction>> GetTransactions();
}
