using MediatR;

namespace Transact.Core.Contracts.User;

public class GetUserQuery(string userId) : IRequest<User>
{
    public string UserId { get; set; } = userId;
}
