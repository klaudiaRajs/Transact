using Transact.Core.Contracts;

namespace Transact.Core.Transactions.Infrastructure;

public interface IUserService
{
    public Task<User> GetUserAsync(string userId); 
}
