using MediatR;
using Transact.Core.Contracts.Product;

namespace Transact.Core.Products.Handlers;

public class GetProductsByIdsHandler(ProductFactory factory) : IRequestHandler<GetProductsByIdsQuery, IEnumerable<Product>>
{
    public async Task<IEnumerable<Product>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
    {
        return factory.GetProductsByIds(request.ProductIds);   
    }
}
