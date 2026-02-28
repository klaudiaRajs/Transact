using MediatR;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.Product;
using Transact.Core.Contracts.Transactions;
using Transact.Core.Contracts.User;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions;

public class TransactionFactory(
    ITransactionRepository transactionRepository,
    ILogger<TransactionFactory> logger) : ITransactionFactory
{
    public async Task<Transaction> GetTransaction(string id)
    {
        return await transactionRepository.GetTransactionByIdAsync(id);
    }

    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var transactions = await transactionRepository.GetTransactionsAsync();
        return transactions;
    }

    public bool CreateTransaction(IIntegrationEvent createTransactionEvent)
    {
        try
        {
            var result = transactionRepository.CreateTransactionAsync(createTransactionEvent).Result;
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating transaction.");
            return false;
        }
    }
}
