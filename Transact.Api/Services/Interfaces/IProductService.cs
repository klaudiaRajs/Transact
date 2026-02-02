using Transact.Core.Contracts;

namespace Transact.Api2.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProducts();
    Task<IEnumerable<Product>> GetProductsByIds(string ids);
}
