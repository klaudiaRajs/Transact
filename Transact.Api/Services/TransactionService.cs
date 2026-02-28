using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.Transaction;
using Transact.Core.Contracts.Transactions;
using Transact.Core.Transactions.Infrastructure;
using ITransactionService = Transact.Api2.Services.Interfaces.ITransactionService;

namespace Transact.Api2.Services;

public class TransactionService(
    ITransactionFactory transactionFactory,
    IOutboxService outboxService,
    ILogger<TransactionService> logger) : ITransactionService
{
    public async Task<Transaction> CreateTransaction([FromBody] CreateTransactionRequest createOutboxRequest)
    {
        try
        {
            await outboxService.SaveOutboxItemAsync(createOutboxRequest,
                ActionTypes.OrchestrateTransactionCreation);
            return new Transaction();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating transaction.");
            throw;
        }
    }

    public Task<IEnumerable<Transaction>> GetAllTransactions()
    {
        return transactionFactory.GetTransactions();
    }

    public Task<Transaction> GetTransactionsById(string id)
    {
        var model = transactionFactory.GetTransaction(id);
        return model;
    }
}
