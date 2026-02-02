using MediatR;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions;

public class TransactionFactory (IMediator mediator, ITransactionRepository transactionRepository) : ITransactionFactory
{
    public async Task<Transaction> GetTransaction(int id, CancellationToken cancellationToken)
    {
        var owner = await mediator.Send(new GetUserQuery("userId_example"), cancellationToken);
        var products = await mediator.Send(new GetProductsByIdsQuery(new List<int> { 1, 2, 3 }), cancellationToken);
        return await Task.FromResult(new Transaction
        {
            Id =  Guid.NewGuid().ToString(), 
            /*Owner = owner,
            Products = products*/
        });
    }
    
    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var transactions = await transactionRepository.GetTransactionsAsync(); 
        return transactions;
    }

    public bool CreateTransaction(CreateTransactionRequest request)
    {
        var result = transactionRepository.CreateTransactionAsync(request).Result;
        return result; 
    }
}
