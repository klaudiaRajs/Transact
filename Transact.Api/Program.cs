using Infrastructure;
using Infrastructure.EventBus;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Transact.Api2.Services;
using Transact.Core.Contracts;
using Transact.Core.Products;
using Transact.Core.Products.Infrastructure;
using Transact.Core.Transactions;
using Transact.Core.Transactions.Infrastructure;
using Transact.Core.Users;
using Transact.Core.Users.Infrastructure;
using IProductService = Transact.Api2.Services.IProductService;

var builder = WebApplication.CreateBuilder(args);

// --- HTTP Client ---
builder.Services.AddHttpClient();

// --- Swagger & Endpoints ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Serilog ---
var loggerConnectionString = builder.Configuration.GetConnectionString("LoggerDb");
Log.Logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
        connectionString: loggerConnectionString,
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true })
    .CreateLogger();
builder.Host.UseSerilog();

// --- DB Contexts & Dependencies ---
var transactionsConnectionString = builder.Configuration.GetConnectionString("TransactionsDb");

builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(transactionsConnectionString, b => b.MigrationsAssembly("Transact.Transactions.Core")));
builder.Services.AddTransactionDependencies(transactionsConnectionString);

var productsConnectionString = builder.Configuration.GetConnectionString("ProductsDb");
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(productsConnectionString, b => b.MigrationsAssembly("Transact.Products.Core")));
builder.Services.AddProductsDependencies(productsConnectionString);

var outboxConnectionString = builder.Configuration.GetConnectionString("OutboxDb");
builder.Services.AddDbContext<OutboxDbContext>(options =>
    options.UseSqlServer(outboxConnectionString, b => b.MigrationsAssembly("Transact.Infrastructure")));
builder.Services.AddOutboxDependencies(outboxConnectionString);

var usersConnectionString = builder.Configuration.GetConnectionString("UsersDb");
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(usersConnectionString, b => b.MigrationsAssembly("Transact.Users.Core")));
builder.Services.AddUsersDependencies(usersConnectionString);

// --- Other Services ---
builder.Services.AddScoped<IProductFactory, ProductFactory>();
builder.Services.AddScoped<IUserFactory, UserFactory>();
builder.Services.AddScoped<ITransactionFactory, TransactionFactory>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// --- Middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- Product Endpoints ---

// GET /product => all products
app.MapGet("/product", async ([FromServices] IProductService productService) =>
{
    var products = await productService.GetProducts();
    return Results.Ok(products);
})
.WithName("GetAllProducts")
.WithOpenApi();

// GET /product/byIds?ids=1&ids=2
/*app.MapGet("/product/byIds", async (
        [FromQuery] IEnumerable<int> ids,
        [FromServices] IProductService productService) =>
{
    var products = await productService.GetProductsByIds(ids);
    return Results.Ok(products);
})
.WithName("GetProductsByIds")
.WithOpenApi();*/

// --- Transaction Endpoints ---

// GET /transaction/{id}
app.MapGet("/transaction/{id:int}", async (
        int id,
        [FromServices] ITransactionService transactionService) =>
{
    var transaction = await transactionService.GetTransactionsById(id);
    return Results.Ok(transaction);
})
.WithName("GetTransactionById")
.WithOpenApi();

// POST /transaction
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

// --- Run ---
await app.RunAsync();
