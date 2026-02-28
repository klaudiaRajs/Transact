using System.Text;
using System.Text.Json;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Contracts.User;

namespace Transact.Core.Users;

public class UserDetailsJobConsumer(IConnection connection, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken); 
        await channel.QueueDeclareAsync(UserMessaging.Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var integrationEvent = JsonSerializer.Deserialize<GetOrCreateUserDetailsIntegrationEvent>(message);

            if (integrationEvent != null)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<UserFactory>();
                var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();
                var userDetails = repository.GetUserById("1"); 
                var userDetailsEvent = new GetOrCreateUserDetailsIntegrationEvent()
                {
                    Payload = JsonSerializer.Serialize(userDetails),
                };
                await outboxService.SaveOutboxItemAsync(userDetailsEvent, ActionTypes.UserReturned); 
                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(UserMessaging.Queue, autoAck: false, consumer, cancellationToken: stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }
}
