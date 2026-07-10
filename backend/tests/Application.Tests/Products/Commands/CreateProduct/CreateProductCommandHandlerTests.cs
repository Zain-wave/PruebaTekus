using PruebaTekus.Application.Products;
using PruebaTekus.Application.Products.Commands.CreateProduct;
using PruebaTekus.Domain.Products;

namespace PruebaTekus.Application.Tests.Products.Commands.CreateProduct;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_AddsProductToRepository()
    {
        var repository = new FakeProductRepository();
        var handler = new CreateProductCommandHandler(repository);
        var command = new CreateProductCommand("Keyboard", 49.99m);

        var id = await handler.Handle(command, CancellationToken.None);

        var stored = await repository.GetByIdAsync(id, CancellationToken.None);
        Assert.NotNull(stored);
        Assert.Equal("Keyboard", stored.Name);
        Assert.Equal(49.99m, stored.Price);
    }

    private class FakeProductRepository : IProductRepository
    {
        private readonly Dictionary<Guid, Product> _products = [];

        public Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            _products[product.Id] = product;

            return Task.CompletedTask;
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            _products.TryGetValue(id, out var product);

            return Task.FromResult(product);
        }
    }
}
