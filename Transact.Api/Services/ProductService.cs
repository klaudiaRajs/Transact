using Transact.Core.Contracts;
using Transact.Core.Products.Infrastructure;

namespace Transact.Api2.Services;

public class ProductService(IProductFactory productFactory) : IProductService
{
    public Task<IEnumerable<Product>> GetProducts()
    {
        var products = productFactory.GetProducts();
        return Task.FromResult(products);
    }
    
    public Task<IEnumerable<Product>> GetProductsByIds(string ids)
    {
        var idList = ids.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var products = productFactory.GetProductsByIds(idList.ToList().Select(int.Parse));
        return Task.FromResult(products);
    }
}
