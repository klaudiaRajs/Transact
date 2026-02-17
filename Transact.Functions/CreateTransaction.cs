using System.Text.Json;
using Infrastructure.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transact.Core.Contracts;

namespace Transact.Functions;

public class CreateTransaction 
{
    private readonly ILogger<CreateTransaction> _logger;
    private readonly IOutboxService _outboxService;

    public CreateTransaction(ILogger<CreateTransaction> logger, IOutboxService outboxService)
    {
        _outboxService = outboxService;
        _logger = logger;
    }

    [Function("CreateTransaction")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        var body = await JsonSerializer.DeserializeAsync<CreateTransactionRequest>(req.Body);
        if (body == null)
        {
            return new BadRequestObjectResult("Invalid request payload.");
        }
        //TODO rozwikłać temat CreateTransactionRequest vs CreateTransactionIntegrationEvent 
        var createTransactionRequest = new CreateTransactionRequest();
        var messageType = "CreateTransactionRequest";
        var result = await _outboxService.SaveOutboxItemAsync(createTransactionRequest, messageType);
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult(result);
    }
}
