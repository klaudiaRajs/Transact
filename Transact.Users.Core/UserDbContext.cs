using Microsoft.EntityFrameworkCore;
using Transact.Core.Contracts.User;

namespace Transact.Core.Users;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    DbSet<User> Users { get; set; }
}
