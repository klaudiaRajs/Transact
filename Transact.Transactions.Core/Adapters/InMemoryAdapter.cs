using Infrastructure.IntegrationEvents;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions.Repositories;

public class InMemoryAdapter(ILogger<InMemoryAdapter> logger) : ITransactionRepository
{
    private readonly List<Transaction> _transactions = new();

    public Task<bool> CreateTransactionAsync(CreateTransactionIntegrationEvent request)
    {
        try
        {
            _transactions.Add(request.Transaction);
            logger.LogInformation($"[InMemory] Transaction created: {request.Transaction.Id} for CorrelationId: ");
    
        } catch (Exception ex)
        {
            logger.LogError(ex, $"[InMemory] Error creating transaction: ");
            return Task.FromResult(false);
        }
        
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Transaction>> GetTransactionsAsync()
    {
        try
        {
            return Task.FromResult<IEnumerable<Transaction>>(_transactions);    
        } catch (Exception ex)
        {
            logger.LogError(ex, "[InMemory] Error retrieving transactions");
            return Task.FromResult<IEnumerable<Transaction>>(new List<Transaction>());
        }
        
    }
}
