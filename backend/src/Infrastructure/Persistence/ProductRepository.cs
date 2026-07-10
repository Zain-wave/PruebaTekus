using Microsoft.EntityFrameworkCore;
using PruebaTekus.Application.Products;
using PruebaTekus.Domain.Products;

namespace PruebaTekus.Infrastructure.Persistence;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return context.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }
}
