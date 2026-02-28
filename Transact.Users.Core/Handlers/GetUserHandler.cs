using MediatR;
using Transact.Core.Contracts.User;

namespace Transact.Core.Users.Handlers;

public class GetUserHandler (UserFactory factory) : IRequestHandler<GetUserQuery, User>
{
    public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(factory.GetUserById(request.UserId));
    }
}
