using PruebaTekus.Application.Providers.Queries.GetProviderById;
using PruebaTekus.Application.Tests.TestDoubles;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.Providers;

public class GetProviderByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingProvider_ReturnsDto()
    {
        var repository = new FakeProviderRepository();
        var provider = new Provider("900123456", "Acme", "https://acme.test", "contact@acme.test");
        EntityTestHelper.SetProperty(provider, nameof(Provider.Id), 7);
        repository.Seed(provider);
        var handler = new GetProviderByIdQueryHandler(repository);

        var dto = await handler.Handle(new GetProviderByIdQuery(7), CancellationToken.None);

        Assert.NotNull(dto);
        Assert.Equal("Acme", dto!.Name);
        Assert.Equal(0, dto.ServicesCount);
    }

    [Fact]
    public async Task Handle_WithMissingProvider_ReturnsNull()
    {
        var repository = new FakeProviderRepository();
        var handler = new GetProviderByIdQueryHandler(repository);

        var dto = await handler.Handle(new GetProviderByIdQuery(1), CancellationToken.None);

        Assert.Null(dto);
    }
}
