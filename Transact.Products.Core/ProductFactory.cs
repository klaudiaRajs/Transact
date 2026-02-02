using Transact.Core.Contracts;
using Transact.Core.Products.Infrastructure;

namespace Transact.Core.Products;

public class ProductFactory : IProductFactory
{
    public IEnumerable<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product { Name = "1984", Price = 9.99m, InStock = true },
            new Product { Name = "To Kill a Mockingbird", Price = 7.99m, InStock = true },
            new Product { Name = "The Great Gatsby", Price = 10.99m, InStock = false },
            new Product { Name = "Moby Dick", Price = 8.99m, InStock = true },
            new Product { Name = "Pride and Prejudice", Price = 6.99m, InStock = true },
            new Product { Name = "War and Peace", Price = 12.99m, InStock = false },
            new Product { Name = "The Catcher in the Rye", Price = 9.49m, InStock = true },
            new Product { Name = "The Hobbit", Price = 11.99m, InStock = true },
            new Product { Name = "Fahrenheit 451", Price = 8.49m, InStock = true },
            new Product { Name = "Jane Eyre", Price = 7.49m, InStock = false }
        };
    }
    
    public IEnumerable<Product> GetProductsByIds(IEnumerable<int> ids)
    {
        var allProducts = GetProducts().ToList();
        return allProducts.Where((product, index) => ids.Contains(index + 1)).ToList();
    }
}

