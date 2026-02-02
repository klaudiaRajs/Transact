using Microsoft.EntityFrameworkCore;
using Transact.Core.Contracts;

namespace Infrastructure;

public class OutboxDbContext(DbContextOptions<OutboxDbContext> options) : DbContext(options)
{
    public DbSet<OutboxItem> Outbox { get; set; }
}
