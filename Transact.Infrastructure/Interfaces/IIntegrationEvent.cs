using MediatR;

namespace Infrastructure.Interfaces;

public interface IIntegrationEvent : INotification
{
    string Id { get; init; }
}
