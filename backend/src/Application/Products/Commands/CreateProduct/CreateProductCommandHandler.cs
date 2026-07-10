using MediatR;
using PruebaTekus.Domain.Products;

namespace PruebaTekus.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductRepository repository) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.Price);

        await repository.AddAsync(product, cancellationToken);

        return product.Id;
    }
}
