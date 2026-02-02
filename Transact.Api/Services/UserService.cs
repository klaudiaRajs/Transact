using Transact.Api2.Services.Interfaces;
using Transact.Core.Contracts;
using Transact.Core.Users.Infrastructure;

namespace Transact.Api2.Services;

public class UserService(IUserFactory userFactory) : IUserService
{

    public Task<IEnumerable<User>> GetUsers()
    {
        var products = userFactory.GetUsers();
        return Task.FromResult(products);
    }
    
    public Task<User> GetUserById(string id)
    {
        var products = userFactory.GetUserById(id);
        return Task.FromResult(products);
    }
}
