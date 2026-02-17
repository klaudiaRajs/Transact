using Microsoft.EntityFrameworkCore;
using Transact.Core.Contracts;

namespace Infrastructure;

public class OutboxDbContext(DbContextOptions<OutboxDbContext> options) : DbContext(options)
{
    public DbSet<TransactionOutboxItem> TransactionsOutbox { get; set; }
    public DbSet<ProductOutboxItem> ProductsOutbox { get; set; }
    public DbSet<UserOutboxItem> UsersOutbox { get; set; }
    public DbSet<OrchestratorOutboxItem> OrchestratorOutbox { get; set; }
}
