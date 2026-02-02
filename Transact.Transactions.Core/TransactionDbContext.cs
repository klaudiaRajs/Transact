using Microsoft.EntityFrameworkCore;
using Transact.Core.Contracts;

namespace Transact.Core.Transactions;

public class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
        // ...inne konfiguracje...
        base.OnModelCreating(modelBuilder);
    }*/
}
