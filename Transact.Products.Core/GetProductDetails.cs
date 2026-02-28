using System.Text.Json;
using Infrastructure.Interfaces;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;

namespace Transact.Core.Products;

public class GetProductDetails (ProductFactory productFactory, IOutboxService outboxService)
{
    public async Task Get(IIntegrationEvent integrationEvent)
    {
        try
        {
            var getProductDetailsIntegrationEvent = new ProductDetailsIntegrationEvent(integrationEvent);
            var productIds = getProductDetailsIntegrationEvent.GetProductIdsFromRequest(); 
            var result = productFactory.GetProductsByIds(productIds);
            getProductDetailsIntegrationEvent.Payload = JsonSerializer.Serialize(result); 
            await outboxService.SaveOutboxItemAsync(getProductDetailsIntegrationEvent, ActionTypes.ReturnProductDetails);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
