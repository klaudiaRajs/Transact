using Infrastructure.Interfaces;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOutboxDependencies(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<OutboxDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IOutboxService, OutboxService>();
        services.AddScoped<IMessageDispatcher, MessageDispatcher>(); 
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

        return services;
    }
}

