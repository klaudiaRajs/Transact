namespace Transact.Core.Contracts.Infrastructure;

public abstract record ActionTypes
{
    public const string OrchestrateTransactionCreation = "OrchestrateTransactionCreation";
    public const string CreateTransactionRequest = "CreateTransactionRequest";
    public const string GetProductDetails = "GetProductDetails";
    public const string ReturnProductDetails = "ReturnProductDetails";
    public const string UserRequested = "UserRequested";
    public const string UserReturned = "UserReturned";
}
