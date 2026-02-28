using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Transact.Api2.Services;
using Transact.Api2.Services.Interfaces;
using Transact.Core.Contracts.Transaction;
using Transact.Core.Products;
using Transact.Core.Transactions;
using Transact.Core.Transactions.Infrastructure;
using Transact.Core.Users;
using Transact.Orchestrator;
using IProductService = Transact.Api2.Services.Interfaces.IProductService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var loggerConnectionString = builder.Configuration.GetConnectionString("LoggerDb");
Log.Logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
        connectionString: loggerConnectionString,
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true })
    .CreateLogger();
builder.Host.UseSerilog();

var transactionsConnectionString = builder.Configuration.GetConnectionString("TransactionsDb");

builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(transactionsConnectionString, b => b.MigrationsAssembly("Transact.Transactions.Core")));
builder.Services.AddTransactionDependencies(transactionsConnectionString);

var productsConnectionString = builder.Configuration.GetConnectionString("ProductsDb");
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(productsConnectionString, b => b.MigrationsAssembly("Transact.Products.Core")));
builder.Services.AddProductsDependencies(productsConnectionString);

var outboxConnectionString = builder.Configuration.GetConnectionString("OutboxDb");
builder.Services.AddDbContextFactory<OutboxDbContext>(options =>
    options.UseSqlServer(outboxConnectionString, b => b.MigrationsAssembly("Transact.Infrastructure")));
builder.Services.AddOutboxDependencies(builder.Configuration, outboxConnectionString);

var usersConnectionString = builder.Configuration.GetConnectionString("UsersDb");
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(usersConnectionString, b => b.MigrationsAssembly("Transact.Users.Core")));
builder.Services.AddUsersDependencies(usersConnectionString);

var orchestratorConnectionString = builder.Configuration.GetConnectionString("OrchestratorDb");
builder.Services.AddDbContext<OrchestratorDbContext>(options =>
    options.UseSqlServer(orchestratorConnectionString, b => b.MigrationsAssembly("Transact.Orchestrator")));
builder.Services.AddOrchestratorDependencies(orchestratorConnectionString);

builder.Services.AddScoped<ProductFactory>();
builder.Services.AddScoped<UserFactory>();
builder.Services.AddScoped<ITransactionFactory, TransactionFactory>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/product", async ([FromServices] IProductService productService) =>
{
    var products = await productService.GetProducts();
    return Results.Ok(products);
})
.WithName("GetAllProducts")
.WithOpenApi();

app.MapGet("/transaction/{id}", async (
        string id,
        [FromServices] ITransactionService transactionService) =>
{
    var transaction = await transactionService.GetTransactionsById(id);
    return Results.Ok(transaction);
})
.WithName("GetTransactionById")
.WithOpenApi();

app.MapPost("/transaction", async (
        [FromBody] CreateTransactionRequest createTransactionRequest,
        [FromServices] ITransactionService transactionService) =>
{
    var correlationId = await transactionService.CreateTransaction(createTransactionRequest);
    return Results.Accepted($"/transaction/status/{correlationId}", new { CorrelationId = correlationId });
})
.WithName("CreateTransaction")
.WithOpenApi();

app.MapGet("/transactions", async (
        [FromServices] ITransactionService transactionService) =>
    {
        var transactions = await transactionService.GetAllTransactions();
        return Results.Ok(transactions);
    })
    .WithName("GetAllTransactions")
    .WithOpenApi();

await app.RunAsync();
