using System.Text.Json;
using Infrastructure.IntegrationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;
using Transact.Core.Transactions.Repositories;

namespace Transact.Core.Transactions;

public class TransactionReporitory : ITransactionRepository
{
    ITransactionRepository _transactionRepository;

    public TransactionReporitory(TransactionDbContext dbContext,
        ILogger<SqlAdapter> sqlLogger,
        ILogger<DriveAdapter> driveRepositoryLogger,
        ILogger<InMemoryAdapter> inMemoryLogger, 
        IConfiguration configuration)
    {
        var storageType = configuration.GetValue<string>("StorageType");
        if (storageType == StorageType.SqlServer || string.IsNullOrEmpty(storageType))
        {
            _transactionRepository = new SqlAdapter(dbContext, sqlLogger);
        }
        else if (storageType == StorageType.Drive)
        {
            _transactionRepository = new DriveAdapter(driveRepositoryLogger);
        }
        else if (storageType == StorageType.InMemory)
        {
            _transactionRepository = new InMemoryAdapter(inMemoryLogger);
        }
    }

    public Task<bool> CreateTransactionAsync(CreateTransactionIntegrationEvent request)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            OwnerId = JsonSerializer.Serialize(request.User),
            ProductsList = JsonSerializer.Serialize(request.Products) 
        };
        request.Transaction = transaction;
        return _transactionRepository.CreateTransactionAsync(request);
    }

    public Task<IEnumerable<Transaction>> GetTransactionsAsync()
    {
        return _transactionRepository.GetTransactionsAsync();
    }
}
