using PruebaTekus.Domain.Products;

namespace PruebaTekus.Application.Products;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
