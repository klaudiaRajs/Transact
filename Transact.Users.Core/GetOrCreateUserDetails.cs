using System.Text.Json;
using Infrastructure.Interfaces;
using Transact.Core.Contracts.Infrastructure;

namespace Transact.Core.Users;

public class GetOrCreateUserDetails (IOutboxService service, UserFactory factory)
{
    public async Task<bool> Get(IIntegrationEvent integrationEvent)
    {
        try
        {
            var result = factory.GetUserById("abc"); 
            integrationEvent.Payload = JsonSerializer.Serialize(result);
            integrationEvent.EventType = ActionTypes.UserReturned; 
            await service.SaveOutboxItemAsync(integrationEvent, ActionTypes.UserReturned);
            return true; 
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false; 
        }

    }
}
