using PruebaTekus.Application.Common.Exceptions;
using PruebaTekus.Application.Services.Commands.UpdateService;
using PruebaTekus.Application.Tests.TestDoubles;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.Services;

public class UpdateServiceCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingService_UpdatesFields()
    {
        var repository = new FakeServiceRepository();
        var service = new Service("Consulting", 100m, providerId: 1);
        EntityTestHelper.SetProperty(service, nameof(Service.Id), 5);
        repository.Seed(service);
        var handler = new UpdateServiceCommandHandler(repository);
        var command = new UpdateServiceCommand(5, "Consulting Plus", 150m);

        await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Consulting Plus", service.Name);
        Assert.Equal(150m, service.HourlyRate);
    }

    [Fact]
    public async Task Handle_WithMissingService_ThrowsNotFoundException()
    {
        var repository = new FakeServiceRepository();
        var handler = new UpdateServiceCommandHandler(repository);
        var command = new UpdateServiceCommand(99, "Consulting", 100m);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
