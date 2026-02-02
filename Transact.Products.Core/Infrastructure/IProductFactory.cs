using Transact.Core.Contracts;

namespace Transact.Core.Products.Infrastructure;

public interface IProductFactory
{
    IEnumerable<Product> GetProducts();
    IEnumerable<Product> GetProductsByIds(IEnumerable<int> ids); 
}
