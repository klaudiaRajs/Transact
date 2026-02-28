using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Products.Handlers;

namespace Transact.Core.Products;

public static class InjectDependencies
{
    public static IServiceCollection AddProductsDependencies(
        this IServiceCollection services, string connectionString)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductsByIdsHandler).Assembly));
        services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(connectionString));
        
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

        services.AddHostedService<ProductDetailsJobConsumer>();
        services.AddScoped<ProductFactory>(); 
        services.AddScoped<GetProductDetails>(); 
        return services;
    }
}
