using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Api2.Services;

public class TransactionService(ITransactionFactory transactionFactory, IOutboxService outboxService, ILogger<TransactionService> logger) : ITransactionService
{
    public async Task<Transaction> CreateTransaction([FromBody]CreateTransactionRequest createOutboxRequest)
    {
        try
        {
            await outboxService.SaveOutboxItemAsync(createOutboxRequest, "CreateTransactionHandler");
            return new Transaction(); 
        } catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating transaction.");
            throw;
        }
    }

    public Task<IEnumerable<Transaction>> GetAllTransactions()
    {
        return transactionFactory.GetTransactions(); 
    }
    
    public Task<Transaction> GetTransactionsById(int id)
    {
        var model = transactionFactory.GetTransaction(id, CancellationToken.None);
        return model; 
    }
}
