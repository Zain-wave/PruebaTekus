using PruebaTekus.Application.Services.Queries.GetServiceById;
using PruebaTekus.Application.Tests.TestDoubles;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.Services;

public class GetServiceByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingService_ReturnsDto()
    {
        var repository = new FakeServiceRepository();
        var provider = new Provider("900123456", "Acme", "https://acme.test", "contact@acme.test", "Colombia");
        EntityTestHelper.SetProperty(provider, nameof(Provider.Id), 1);
        var service = new Service("Consulting", 100m, provider.Id);
        EntityTestHelper.SetProperty(service, nameof(Service.Id), 5);
        EntityTestHelper.SetProperty(service, nameof(Service.Provider), provider);
        repository.Seed(service);
        var handler = new GetServiceByIdQueryHandler(repository);

        var dto = await handler.Handle(new GetServiceByIdQuery(5), CancellationToken.None);

        Assert.NotNull(dto);
        Assert.Equal("Consulting", dto!.Name);
        Assert.Equal("Acme", dto.ProviderName);
    }

    [Fact]
    public async Task Handle_WithMissingService_ReturnsNull()
    {
        var repository = new FakeServiceRepository();
        var handler = new GetServiceByIdQueryHandler(repository);

        var dto = await handler.Handle(new GetServiceByIdQuery(1), CancellationToken.None);

        Assert.Null(dto);
    }
}
