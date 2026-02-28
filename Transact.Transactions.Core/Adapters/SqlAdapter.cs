using Microsoft.Extensions.Logging;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Contracts.Transactions;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions.Adapters;

public class SqlAdapter(TransactionDbContext dbContext, ILogger<SqlAdapter> logger) : ITransactionRepository
{
    public async Task<bool> CreateTransactionAsync(IIntegrationEvent request)
    {
        try
        {
            var items = new CreateTransactionIntegrationEvent(request);
            dbContext.Transactions.Add(items.Transaction);
            var result = await dbContext.SaveChangesAsync();
            logger.LogInformation($"Saved transaction item to module db for: , service: {nameof(Transact.Core.Transactions)}");
            return result.Equals(1);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public Task<IEnumerable<Transaction>> GetTransactionsAsync()
    {
        var transactions = dbContext.Transactions; 
        return Task.FromResult<IEnumerable<Transaction>>(transactions); 
    }

    public Task<Transaction> GetTransactionByIdAsync(string id)
    {
        return Task.FromResult(dbContext.Transactions.FirstOrDefault(a=> a.Id == id));
    }
}
