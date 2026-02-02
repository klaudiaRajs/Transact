using Infrastructure.EventBus;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions;

public static class DependencyInjection
{
    public static IServiceCollection AddTransactionDependencies(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TransactionDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<ITransactionRepository, RepositoryAdapter>(); 
        services.AddScoped<
            IMessageHandler<CreateTransactionIntegrationEvent>,
            CreateTransactionHandler
        >();
        
        
        return services;
    }
}

