using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Transact.Core.Users.Handlers;

namespace Transact.Core.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersDependencies(
        this IServiceCollection services, string connectionString)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserHandler).Assembly));
        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}
