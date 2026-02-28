using Transact.Api2.Services.Interfaces;
using Transact.Core.Contracts.User;
using Transact.Core.Users;

namespace Transact.Api2.Services;

public class UserService(UserFactory userFactory) : IUserService
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
