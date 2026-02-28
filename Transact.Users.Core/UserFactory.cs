using Transact.Core.Contracts.User;

namespace Transact.Core.Users;

public class UserFactory
{
    public IEnumerable<User> GetUsers()
    {
        return new List<User>
        {
            new User { Id = "abc", Name = "John", Surname = "Doe", Active = true },
            new User { Id = "abc3", Name = "Jane", Surname = "Smith", Active = true },
            new User { Id = "abc4", Name = "Bob", Surname = "Brown", Active = false }
        };
    }
    
    public User GetUserById(string id)
    {
        return new User { Id = "abvcges", Name = "John", Surname = "Doe", Active = true }; 
    }
}
