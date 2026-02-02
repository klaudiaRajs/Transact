using MediatR;
using Transact.Core.Contracts;
using Transact.Core.Users.Infrastructure;

namespace Transact.Core.Users.Handlers;

public class GetUserHandler (IUserFactory factory) : IRequestHandler<GetUserQuery, User>
{
    public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(factory.GetUserById(request.UserId));
    }
}
