using System.Text.Json.Serialization;

namespace Transact.Core.Contracts.Transaction;

public class CreateTransactionRequest
{
    [JsonPropertyName("user")]
    public User.User User { get; set; }
    [JsonPropertyName("productIds")]
    public string ProductIds { get; set; }
}
