using System.Text.Json;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions.Repositories;

public class SqlRepository(TransactionDbContext dbContext, ILogger<SqlRepository> logger) : ITransactionRepository
{
    public async Task<bool> CreateTransactionAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                //OWNER jako snapshot 
                OwnerId = request.UserId,
                ProductsList = JsonSerializer.Serialize(request.ProductIds)
            };

            //Jak user nie istnieje to machnij trigger do outboxa na utworzenie usera 
            dbContext.Transactions.Add(transaction);
            var result = await dbContext.SaveChangesAsync();
            logger.LogInformation($"Saved transaction item to module db for: {request.CorrelationId}, service: {nameof(Transact.Core.Transactions)}");
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
