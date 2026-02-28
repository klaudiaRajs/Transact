using Microsoft.EntityFrameworkCore;
using Transact.Core.Contracts.Infrastructure;

namespace Infrastructure;

public class OutboxDbContext(DbContextOptions<OutboxDbContext> options) : DbContext(options)
{
    public DbSet<ProductOutboxItem> ProductsOutbox { get; set; }
    public DbSet<UserOutboxItem> UsersOutbox { get; set; }
    public DbSet<OrchestratorOutboxItem> OrchestratorOutbox { get; set; }
    public DbSet<TransactionDataOutboxItem> TransactionDataOutbox { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TransactionDataOutboxItem>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("TransactionData", "dbo");
        });
    }

}
