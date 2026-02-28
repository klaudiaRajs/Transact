using MediatR;

namespace Transact.Core.Contracts.Product;

public class GetProductsByIdsQuery(IEnumerable<int> productIds) : IRequest<IEnumerable<Product>>
{
    public IEnumerable<int> ProductIds { get; set; } = productIds;
}
