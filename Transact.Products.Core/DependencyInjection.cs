using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Transact.Core.Products.Handlers;

namespace Transact.Core.Products;

public static class DependencyInjection
{
    public static IServiceCollection AddProductsDependencies(
        this IServiceCollection services, string connectionString)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductsByIdsHandler).Assembly));
        services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}
