using Microsoft.EntityFrameworkCore;
using Transact.Orchestrator.Transaction;

namespace Transact.Orchestrator;

public class OrchestratorDbContext(DbContextOptions<OrchestratorDbContext> options) : DbContext(options)
{
    public DbSet<OrchestratorTransaction> OrchestratorTransactions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrchestratorTransaction>()
            .HasKey(o => o.Id); 
        modelBuilder.Entity<OrchestratorTransaction>()
            .Property(o => o.Id)
            .ValueGeneratedOnAdd();
    }
}
