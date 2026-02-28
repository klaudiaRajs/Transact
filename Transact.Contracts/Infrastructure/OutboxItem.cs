namespace Transact.Core.Contracts.Infrastructure;

public class OutboxItem
{
    public string Id { get; set; }
    public string Type { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string Payload { get; set; }
    public string CorrelationId { get; set; }

    public OutboxItem(IIntegrationEvent integrationEvent)
    {
        Id = Guid.NewGuid().ToString();
        OccurredOn = DateTime.UtcNow;
        Type = integrationEvent.EventType;
        Payload = integrationEvent.Payload;
        CorrelationId = integrationEvent.CorrelationId; 
    }

    public OutboxItem()
    {
        
    }
}

public class UserOutboxItem : OutboxItem
{
    public UserOutboxItem(IIntegrationEvent integrationEvent)
    {
        Id = Guid.NewGuid().ToString();
        OccurredOn = DateTime.UtcNow;
        Type = integrationEvent.EventType;
        Payload = integrationEvent.Payload;
        CorrelationId = integrationEvent.CorrelationId; 
    }

    public UserOutboxItem()
    {
        
    }
}

public class ProductOutboxItem : OutboxItem
{
    public ProductOutboxItem(IIntegrationEvent integrationEvent)
    {
        Id = Guid.NewGuid().ToString();
        OccurredOn = DateTime.UtcNow;
        Type = integrationEvent.EventType;
        Payload = integrationEvent.Payload;
        CorrelationId = integrationEvent.CorrelationId; 
    }

    public ProductOutboxItem()
    {
        
    }
}

public class OrchestratorOutboxItem : OutboxItem
{
    public OrchestratorOutboxItem(IIntegrationEvent integrationEvent)
    {
        Id = Guid.NewGuid().ToString();
        OccurredOn = DateTime.UtcNow;
        Type = integrationEvent.EventType;
        Payload = integrationEvent.Payload;
        CorrelationId = integrationEvent.CorrelationId; 
    }

    public OrchestratorOutboxItem()
    {
        
    }
}

public class TransactionDataOutboxItem : OutboxItem
{
    public TransactionDataOutboxItem(IIntegrationEvent integrationEvent)
    {
        Id = Guid.NewGuid().ToString();
        OccurredOn = DateTime.UtcNow;
        Type = integrationEvent.EventType;
        Payload = integrationEvent.Payload;
        CorrelationId = integrationEvent.CorrelationId; 
    }

    public TransactionDataOutboxItem()
    {
        
    }
    public string UserPayload { get; set; }
    public string ProductPayload { get; set; }
}
