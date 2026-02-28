using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Users.Handlers;

namespace Transact.Core.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersDependencies(
        this IServiceCollection services, string connectionString)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserHandler).Assembly));
        services.AddDbContext<UserDbContext>(options =>
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

        services.AddHostedService<UserDetailsJobConsumer>(); 
        services.AddScoped<GetOrCreateUserDetails>();
        return services;
    }
}
