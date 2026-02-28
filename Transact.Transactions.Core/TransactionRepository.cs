using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Contracts.Transactions;
using Transact.Core.Transactions.Adapters;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions;

public class TransactionRepository : ITransactionRepository
{
    readonly ITransactionRepository transactionRepository;

    public TransactionRepository(TransactionDbContext dbContext,
        ILogger<SqlAdapter> sqlLogger,
        ILogger<DriveAdapter> driveRepositoryLogger,
        ILogger<InMemoryAdapter> inMemoryLogger, 
        IConfiguration configuration)
    {
        var storageType = configuration.GetValue<string>("StorageType");
        if (storageType == StorageType.SqlServer || string.IsNullOrEmpty(storageType))
        {
            transactionRepository = new SqlAdapter(dbContext, sqlLogger);
        }
        else if (storageType == StorageType.Drive)
        {
            transactionRepository = new DriveAdapter(driveRepositoryLogger);
        }
        else if (storageType == StorageType.InMemory)
        {
            transactionRepository = new InMemoryAdapter(inMemoryLogger);
        }
    }

    public Task<bool> CreateTransactionAsync(IIntegrationEvent request)
    {
        var createTransactionEvent = new CreateTransactionIntegrationEvent(request);
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            OwnerId = JsonSerializer.Serialize(createTransactionEvent.User),
            ProductsList = JsonSerializer.Serialize(createTransactionEvent.Products) 
        };
        createTransactionEvent.Transaction = transaction;
        return transactionRepository.CreateTransactionAsync(request);
    }

    public Task<IEnumerable<Transaction>> GetTransactionsAsync()
    {
        return transactionRepository.GetTransactionsAsync();
    }

    public Task<Transaction> GetTransactionByIdAsync(string id)
    {
        return transactionRepository.GetTransactionByIdAsync(id);
    }
}
