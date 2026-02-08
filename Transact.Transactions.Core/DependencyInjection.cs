using Infrastructure.IntegrationEvents;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Transact.Core.Contracts;
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
        services.AddSingleton<IConnection>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            var factory = new ConnectionFactory
            {
                Uri = new Uri(options.Uri),
                ClientProvidedName = options.ClientName
            };
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        services.AddHostedService<TransactionCreatedConsumerJob>(); 
        return services;
    }
}

