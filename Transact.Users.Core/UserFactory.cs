using Transact.Core.Contracts;
using Transact.Core.Users.Infrastructure;

namespace Transact.Core.Users;

public class UserFactory : IUserFactory
{
    public IEnumerable<User> GetUsers()
    {
        return new List<User>
        {
            new User { Id = 1, Name = "John", Surname = "Doe", Active = true },
            new User { Id = 2, Name = "Jane", Surname = "Smith", Active = true },
            new User { Id = 3, Name = "Bob", Surname = "Brown", Active = false }
        };
    }
    
    public User GetUserById(string id)
    {
        return new User { Id = 1, Name = "John", Surname = "Doe", Active = true }; 
    }
}
