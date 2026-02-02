using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;
using Transact.Core.Transactions.Repositories;

namespace Transact.Core.Transactions;

public class RepositoryAdapter : ITransactionRepository
{
    ITransactionRepository _transactionRepository;

    public RepositoryAdapter(TransactionDbContext dbContext,
        ILogger<SqlRepository> sqlLogger,
        ILogger<DriveTransactionRepository> driveRepositoryLogger,
        ILogger<InMemoryTransactionRepository> inMemoryLogger, 
        IConfiguration configuration)
    {
        var storageType = configuration.GetValue<string>("StorageType");
        if (storageType == StorageType.SqlServer || string.IsNullOrEmpty(storageType))
        {
            _transactionRepository = new SqlRepository(dbContext, sqlLogger);
        }
        else if (storageType == StorageType.Drive)
        {
            _transactionRepository = new DriveTransactionRepository(driveRepositoryLogger);
        }
        else if (storageType == StorageType.InMemory)
        {
            _transactionRepository = new InMemoryTransactionRepository(inMemoryLogger);
        }
    }

    public Task<bool> CreateTransactionAsync(CreateTransactionRequest request)
    {
        return _transactionRepository.CreateTransactionAsync(request);
    }

    public Task<IEnumerable<Transaction>> GetTransactionsAsync()
    {
        return _transactionRepository.GetTransactionsAsync();
    }
}
