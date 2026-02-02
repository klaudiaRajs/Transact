using Transact.Core.Contracts;

namespace Transact.Core.Users.Infrastructure;

public interface IUserFactory
{
    IEnumerable<User> GetUsers();
    User GetUserById(string id);
}
