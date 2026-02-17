namespace Transact.Core.Contracts.Infrastructure;

public record ActionTypes
{
    public const string OrchestrateTransactionCreation = "OrchestrateTransactionCreation";
    public const string CreateTransactionRequest = "CreateTransactionRequest";
    public const string GetProductDetails = "GetProductDetails";
    public const string ReturnProductDetails = "ReturnProductDetails";
    public const string ProductsDetailsReturned = "ProductsDetailsReturned";
    public const string UserRequested = "UserRequested";
    public const string UserReturned = "UserReturned";
}
