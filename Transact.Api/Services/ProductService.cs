using Transact.Api2.Services.Interfaces;
using Transact.Core.Contracts.Product;
using Transact.Core.Products;

namespace Transact.Api2.Services;

public class ProductService(ProductFactory productFactory) : IProductService
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
