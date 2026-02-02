using Infrastructure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();
var connectionString = builder.Configuration["OutboxDb"];
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("OutboxDb connection string is not configured.");
}
builder.Services.AddOutboxDependencies(connectionString);

builder.Build().Run();
