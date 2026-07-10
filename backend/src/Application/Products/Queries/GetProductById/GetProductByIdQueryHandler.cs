using MediatR;

namespace PruebaTekus.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductRepository repository) : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        return product is null ? null : new ProductDto(product.Id, product.Name, product.Price);
    }
}
