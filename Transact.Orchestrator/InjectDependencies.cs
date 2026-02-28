using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Transact.Orchestrator.Transaction;

namespace Transact.Orchestrator;

public static class InjectDependencies
{
    public static IServiceCollection AddOrchestratorDependencies(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<OrchestratorDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<OrchestrateTransactionCreation>();
        services.AddScoped<OrchestrateRepository>();
        services.AddScoped<OrchestrateTransaction>();
        services.AddHostedService<OrchestrateTransactionCreationJobConsumer>(); 
        return services;
    }
}
