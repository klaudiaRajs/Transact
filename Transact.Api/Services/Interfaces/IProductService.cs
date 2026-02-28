using Transact.Core.Contracts.Product;

namespace Transact.Api2.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProducts();
    Task<IEnumerable<Product>> GetProductsByIds(string ids);
}
