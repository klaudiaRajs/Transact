using Microsoft.Extensions.Configuration;
using Transact.Core.Contracts.Infrastructure;

namespace Infrastructure.Messaging;

public class DispatchMessage : IDispatchMessage
{
    private readonly IDispatchMessage? _dispatchMessage;

    public DispatchMessage(IConfiguration configuration, RabbitMqAdapter rabbitMqAdapter, MessageDispatcherAdapter messageDispatcherAdapter, ProjectReferenceAdapter projectReferenceAdapter)
    {
        var messagingType = configuration.GetValue<string>("MessagingType");
        if (messagingType == MessagingType.RabbitMQ || string.IsNullOrEmpty(messagingType))
        {
            _dispatchMessage = rabbitMqAdapter;
        }
        else if (messagingType == MessagingType.MessageDispatcher)
        {
            _dispatchMessage = messageDispatcherAdapter;
        }
        else if (messagingType == MessagingType.ProjectReference)
        {
            _dispatchMessage = projectReferenceAdapter;
        }
    }
    
    public Task Dispatch(IIntegrationEvent integrationEvent, CancellationToken ct)
    {
        //Shandluj wyjątek, jeśli _dispatchMessage jest null, co oznacza, że nie został poprawnie skonfigurowany
        return _dispatchMessage.Dispatch(integrationEvent, ct);
    }
}
