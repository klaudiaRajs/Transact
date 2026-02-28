using Microsoft.EntityFrameworkCore;
using Transact.Core.Contracts.Transactions;

namespace Transact.Core.Transactions;

public class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }
}
