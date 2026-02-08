using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using RabbitMQ.Client;
using Transact.Core.Contracts;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOutboxDependencies(
        this IServiceCollection services, IConfiguration configuration, string connectionString)
    {
        services.AddDbContext<OutboxDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IOutboxService, OutboxService>();
        services.AddHostedService<TransactionRabbitMqInitializer>(); 
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(OutboxProcessorJob));

            configure
                .AddJob<OutboxProcessorJob>(jobKey, configureJob => configureJob.StoreDurably())
                .AddTrigger(
                    trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(20).WithRepeatCount(10)));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        services.Configure<RabbitMqOptions>(
            configuration.GetSection("RabbitMq"));

        services.AddSingleton<IConnection>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

            var factory = new ConnectionFactory
            {
                Uri = new Uri(options.Uri),
                ClientProvidedName = options.ClientName
            };
            return factory.CreateConnectionAsync()
                .GetAwaiter()
                .GetResult(); 
        });
        services.AddSingleton<RabbitMqEventBus>(); 
        return services;
    }
}

