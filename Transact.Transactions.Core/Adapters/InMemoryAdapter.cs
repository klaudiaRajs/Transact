using Microsoft.Extensions.Logging;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Contracts.Transactions;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions.Adapters;

public class InMemoryAdapter(ILogger<InMemoryAdapter> logger) : ITransactionRepository
{
    private readonly List<Transaction> _transactions = new();

    public Task<bool> CreateTransactionAsync(IIntegrationEvent request)
    {
        try
        {
            var item = new CreateTransactionIntegrationEvent(request); 
            _transactions.Add(item.Transaction);
            logger.LogInformation($"[InMemory] Transaction created: {item.Transaction.Id} for CorrelationId: ");
    
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

    public Task<Transaction> GetTransactionByIdAsync(string id)
    {
        var item = _transactions.SingleOrDefault(x => x.Id == id);
        return Task.FromResult<Transaction>(item); 
    }
}
