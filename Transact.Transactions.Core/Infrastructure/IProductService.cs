using Transact.Core.Contracts;

namespace Transact.Core.Transactions.Infrastructure;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsByIds(IEnumerable<int> ids); 
}