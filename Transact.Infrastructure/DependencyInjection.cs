using Infrastructure.Interfaces;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using RabbitMQ.Client;
using Transact.Core.Contracts.Infrastructure;

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
        services.AddScoped<RabbitMqAdapter>();
        services.AddScoped<ProjectReferenceAdapter>();
        services.AddScoped<MessageDispatcherAdapter>();
        services.AddScoped<IDispatchMessage, DispatchMessage>();
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(OutboxProcessorJob));

            configure
                .AddJob<OutboxProcessorJob>(jobKey, configureJob => configureJob.StoreDurably())
                .AddTrigger(
                    trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(40).RepeatForever()));

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

