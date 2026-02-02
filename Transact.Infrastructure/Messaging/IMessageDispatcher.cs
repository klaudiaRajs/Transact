namespace Infrastructure.Messaging;

public interface IMessageDispatcher
{
    Task DispatchAsync(object message, CancellationToken ct);
}
