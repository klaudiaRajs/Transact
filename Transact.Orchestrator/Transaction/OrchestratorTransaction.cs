using System.ComponentModel.DataAnnotations;

namespace Transact.Orchestrator.Transaction;

public class OrchestratorTransaction
{
    [Key]
    public string Id { get; set; }
    public string EventToRaise { get; set; }
    public string CorrelationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string Payload { get; set; }
    public string Status { get; set; }
}
