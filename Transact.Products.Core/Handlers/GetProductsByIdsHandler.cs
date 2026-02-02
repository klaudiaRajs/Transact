using MediatR;
using Transact.Core.Contracts;
using Transact.Core.Products.Infrastructure;

namespace Transact.Core.Products.Handlers;

public class GetProductsByIdsHandler(IProductFactory factory) : IRequestHandler<GetProductsByIdsQuery, IEnumerable<Product>>
{
    public async Task<IEnumerable<Product>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
    {
        return factory.GetProductsByIds(request.ProductIds);   
    }
}
