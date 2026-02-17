using System.Text.Json.Serialization;

namespace Transact.Core.Contracts;

public class CreateTransactionRequest
{
    [JsonPropertyName("user")]
    public User User { get; set; }
    [JsonPropertyName("productIds")]
    public string ProductIds { get; set; }
    //public string MessageType { get; set; }
    //public string Payload { get; set; }

    /*public CreateTransactionRequest(User user, string productIds, string messageType = null)
    {
        User = user;
        ProductIds = productIds;
    }*/
}
