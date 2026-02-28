namespace Transact.Core.Contracts.Infrastructure;

public static class StorageType
{
    public static string InMemory = "inmemory";
    public static string SqlServer = "sqlserver";
    public static string Drive = "drive";
}

public static class MessagingType
{
    public static string RabbitMQ = "rabbitmq";
    public static string ProjectReference = "pr";
    public static string MessageDispatcher = "dispatcher";
}
