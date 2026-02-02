namespace Infrastructure.Messaging;

public interface IMessageHandler<in TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken ct);
}
