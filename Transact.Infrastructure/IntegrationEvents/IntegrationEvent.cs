using Infrastructure.Interfaces;

namespace Infrastructure.EventBus;

public record IntegrationEvent(string Id) : IIntegrationEvent;
