namespace Transact.Core.Contracts.Transaction;

public static class TransactionMessaging
{
    public const string Exchange = "transactions.exchange";
    public const string Queue = "transactions.queue";
    public const string RoutingKey = "transaction.created";
}
