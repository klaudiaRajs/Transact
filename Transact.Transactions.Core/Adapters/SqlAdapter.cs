using System.Text.Json;
using Infrastructure.IntegrationEvents;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions.Repositories;

public class SqlAdapter(TransactionDbContext dbContext, ILogger<SqlAdapter> logger) : ITransactionRepository
{
    public async Task<bool> CreateTransactionAsync(CreateTransactionIntegrationEvent request)
    {
        try
        {
            dbContext.Transactions.Add(request.Transaction);
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
}
