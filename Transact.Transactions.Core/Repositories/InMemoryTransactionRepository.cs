using Microsoft.Extensions.Logging;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions.Repositories;

public class InMemoryTransactionRepository(ILogger<InMemoryTransactionRepository> logger) : ITransactionRepository
{
    private readonly List<Transaction> _transactions = new();

    public Task<bool> CreateTransactionAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                OwnerId = request.UserId,
                ProductsList = string.Join(",", request.ProductIds)
            };

            _transactions.Add(transaction);
            logger.LogInformation($"[InMemory] Transaction created: {request.CorrelationId}");
    
        } catch (Exception ex)
        {
            logger.LogError(ex, $"[InMemory] Error creating transaction: {request.CorrelationId}");
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
