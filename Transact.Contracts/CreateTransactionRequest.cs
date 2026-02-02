using System.Text.Json.Serialization;

namespace Transact.Core.Contracts;

public class CreateTransactionRequest
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; }
    [JsonPropertyName("productIds")]
    public string ProductIds { get; set; }
    public string CorrelationId { get; set; }
    public string MessageType { get; set; }

    public CreateTransactionRequest(string userId, string productIds, string messageType = null)
    {
        UserId = userId;
        ProductIds = productIds;
        MessageType = messageType;
    }
}
