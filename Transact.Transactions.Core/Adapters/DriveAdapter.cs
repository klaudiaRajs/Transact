using Infrastructure.IntegrationEvents;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions.Repositories;

public class DriveAdapter(ILogger<DriveAdapter> logger) : ITransactionRepository
{
    public async Task<bool> CreateTransactionAsync(CreateTransactionIntegrationEvent request)
    {
        var filePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.FullName, "Db", $"{Guid.NewGuid()}.json");
        var jsonData = System.Text.Json.JsonSerializer.Serialize(request.Transaction);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await File.WriteAllTextAsync(filePath, jsonData);
        
        return true;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
    {
        var directoryPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.FullName, "Db");
        var transactions = new List<Transaction>();

        foreach (var file in Directory.GetFiles(directoryPath, "*.json"))
        {
            var jsonData = await File.ReadAllTextAsync(file);
            var transaction = System.Text.Json.JsonSerializer.Deserialize<Transaction>(jsonData);
            if (transaction != null)
            {
                transactions.Add(transaction);
            }
        }

        return transactions;
    }
}
