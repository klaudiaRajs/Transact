namespace Transact.Core.Contracts;

public class OutboxItem
{
    public string Id { get; set; }
    public string Type { get; set; }
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string Payload { get; set; }
    public string CorrelationId { get; set; }
}
