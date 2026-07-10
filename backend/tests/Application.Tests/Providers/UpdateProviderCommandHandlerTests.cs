using PruebaTekus.Application.Common.Exceptions;
using PruebaTekus.Application.Providers.Commands.UpdateProvider;
using PruebaTekus.Application.Tests.TestDoubles;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Tests.Providers;

public class UpdateProviderCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingProvider_UpdatesFields()
    {
        var repository = new FakeProviderRepository();
        var provider = new Provider("900123456", "Acme", "https://acme.test", "contact@acme.test", "Colombia");
        EntityTestHelper.SetProperty(provider, nameof(Provider.Id), 1);
        repository.Seed(provider);
        var handler = new UpdateProviderCommandHandler(repository);
        var command = new UpdateProviderCommand(1, "Acme Renamed", "https://acme.new", "new@acme.test", "Mexico");

        await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Acme Renamed", provider.Name);
        Assert.Equal("new@acme.test", provider.Email);
        Assert.Equal("Mexico", provider.Country);
    }

    [Fact]
    public async Task Handle_WithMissingProvider_ThrowsNotFoundException()
    {
        var repository = new FakeProviderRepository();
        var handler = new UpdateProviderCommandHandler(repository);
        var command = new UpdateProviderCommand(99, "Acme", "https://acme.test", "contact@acme.test", "Colombia");

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}
